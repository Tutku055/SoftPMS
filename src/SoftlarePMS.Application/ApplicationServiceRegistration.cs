using Microsoft.Extensions.DependencyInjection;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Application.Services.Implementations;

namespace SoftlarePMS.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
