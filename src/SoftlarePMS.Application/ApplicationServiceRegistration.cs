using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SoftlarePMS.Application.Common.Behaviors;
using SoftlarePMS.Application.Services.Implementations;
using SoftlarePMS.Application.Services.Interfaces;

namespace SoftlarePMS.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // AutoMapper — scans this assembly for all Profile subclasses
        services.AddAutoMapper(assembly);

        // MediatR — registers all IRequestHandler<,> implementations
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // FluentValidation — registers all AbstractValidator<T> implementations
        services.AddValidatorsFromAssembly(assembly);

        // Pipeline behaviors — order matters: Logging wraps Validation wraps Handler
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Legacy service facades (forward calls to MediatR internally)
        services.AddScoped<IEmployeeService, EmployeeService>();

        // Non-CQRS services (simple CRUD; CQRS migration planned for a follow-up sprint)
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
