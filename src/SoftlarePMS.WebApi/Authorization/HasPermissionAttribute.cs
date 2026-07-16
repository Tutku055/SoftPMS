using Microsoft.AspNetCore.Mvc.Filters;

namespace SoftlarePMS.WebApi.Authorization;

/// <summary>
/// Decorates an action or controller to require a specific permission claim
/// in the current user's JWT before the action is invoked.
///
/// Usage: [HasPermission("Employees.Read")]
///
/// Implements <see cref="IFilterFactory"/> so that ASP.NET Core's filter pipeline
/// can resolve <see cref="HasPermissionFilter"/> from the DI container at dispatch time,
/// with the required permission passed as a constructor argument.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class HasPermissionAttribute(string permission) : Attribute, IFilterFactory
{
    public string Permission { get; } = permission;

    // The filter itself has no mutable state beyond the permission string → safe to reuse.
    public bool IsReusable => true;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) =>
        new HasPermissionFilter(Permission);
}
