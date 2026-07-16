using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Persistence.Context;
using SoftlarePMS.Persistence.Interceptors;

namespace SoftlarePMS.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        string connectionString)
    {
        // Register the audit interceptor as a scoped service so it can resolve
        // ICurrentUserService (which is also scoped — tied to the HTTP request)
        services.AddScoped<AuditSaveChangesInterceptor>();

        services.AddDbContext<SoftlarePMSDbContext>((provider, options) =>
        {
            options.UseSqlServer(connectionString);

            // Wire the audit interceptor into the DbContext pipeline
            var auditInterceptor = provider.GetRequiredService<AuditSaveChangesInterceptor>();
            options.AddInterceptors(auditInterceptor);
        });

        // Register IApplicationDbContext → SoftlarePMSDbContext (same scoped instance)
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<SoftlarePMSDbContext>());

        return services;
    }
}
