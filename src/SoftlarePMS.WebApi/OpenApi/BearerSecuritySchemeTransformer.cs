using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace SoftlarePMS.WebApi.OpenApi;

/// <summary>
/// OpenAPI document transformer that injects a JWT Bearer security scheme
/// into the generated spec and marks every operation as requiring it.
/// Enables the "Authorize" button in Scalar so tokens can be sent automatically.
/// </summary>
internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    private const string SchemeName = "Bearer";

    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var schemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (!schemes.Any(s => s.Name == JwtBearerDefaults.AuthenticationScheme))
            return;

        // Register the Bearer security scheme in the components section
        document.Components ??= new OpenApiComponents();

        // Ensure the SecuritySchemes dictionary is initialized
        if (document.Components.SecuritySchemes == null)
        {
            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
        }

        // Add the Bearer security scheme to the OpenAPI document
        document.Components.SecuritySchemes[SchemeName] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT access token. Obtain one via POST /api/auth/login.",
            In = ParameterLocation.Header
        };

        // OpenApiSecurityRequirement in v2 keys on OpenApiSecuritySchemeReference with List<string> scopes
        var requirement = new OpenApiSecurityRequirement();
        requirement.Add(
            new OpenApiSecuritySchemeReference(SchemeName, document),
            new List<string>());

        document.Security ??= [];
        document.Security.Add(requirement);
    }
}
