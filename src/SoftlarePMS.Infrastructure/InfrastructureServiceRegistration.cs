using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Infrastructure.Services;
using SoftlarePMS.Infrastructure.Settings;

namespace SoftlarePMS.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind and eagerly validate JwtSettings at startup via DataAnnotations
        services
            .AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(nameof(JwtSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpContextAccessor();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
