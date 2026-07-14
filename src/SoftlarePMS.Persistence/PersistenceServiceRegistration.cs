using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Persistence.Context;
using SoftlarePMS.Domain.Repositories;
using SoftlarePMS.Domain.UnitOfWork;
using SoftlarePMS.Persistence.Repositories;
using SoftlarePMS.Persistence.UnitOfWork;

namespace SoftlarePMS.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SoftlarePMSDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        services.AddScoped<IUnitOfWork, SoftlarePMS.Persistence.UnitOfWork.UnitOfWork>();

        return services;
    }
}
