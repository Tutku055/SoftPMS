using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SoftlarePMS.Application;
using SoftlarePMS.Infrastructure;
using SoftlarePMS.Persistence;
using SoftlarePMS.WebApi.Middleware;
using SoftlarePMS.WebApi.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ── Layer service registrations ──────────────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured."));

// ── JWT Bearer authentication ─────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secretKey  = jwtSection["SecretKey"]
    ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer           = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidateAudience         = true,
            ValidAudience            = jwtSection["Audience"],
            ValidateLifetime         = true,
            ClockSkew                = TimeSpan.Zero, // Tokens expire precisely on time — no tolerance
        };

        // Return structured JSON on 401 instead of an empty response
        options.Events = new JwtBearerEvents
        {
            OnChallenge = ctx =>
            {
                ctx.HandleResponse();
                ctx.Response.StatusCode  = StatusCodes.Status401Unauthorized;
                ctx.Response.ContentType = "application/json";
                return ctx.Response.WriteAsync(
                    """{"status":401,"title":"Unauthorized","message":"A valid Bearer token is required."}""");
            },
        };
    });

builder.Services.AddAuthorization();

// ── CORS (permissive for development — tighten in production) ─────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── OpenAPI (built-in .NET 10) + Scalar ──────────────────────────────────────
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info = new()
        {
            Title       = "SoftlarePMS API",
            Version     = "v1",
            Description = "Personnel Management System — RESTful API",
        };
        return Task.CompletedTask;
    });

    // Add JWT Bearer security scheme to every generated document
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// ── Build ─────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Seed database at startup (migrations + seed data) ────────────────────────
try
{
    await DatabaseSeeder.SeedAsync(app.Services);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "An unhandled exception occurred during database seeding.");
    // Optionally throw, but usually we just want to log it and let the app start or fail gracefully
    throw;
}

// ── HTTP pipeline ─────────────────────────────────────────────────────────────

// Global exception handler — registered first to catch all downstream exceptions
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // Expose the raw OpenAPI JSON document
    app.MapOpenApi();

    // Scalar UI (replaces Swagger UI) at /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.Title           = "SoftlarePMS API";
        options.Theme           = ScalarTheme.Purple;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
