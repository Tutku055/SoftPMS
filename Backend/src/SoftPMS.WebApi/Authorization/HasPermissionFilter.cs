using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SoftPMS.WebApi.Authorization;

/// <summary>
/// Action filter that enforces permission-based RBAC.
/// Checks whether the authenticated user's JWT contains a "permission" claim
/// matching the value declared on the <see cref="HasPermissionAttribute"/>.
/// Returns 401 when the user is not authenticated, 403 when the permission is absent.
/// </summary>
public sealed class HasPermissionFilter(params string[] permissions) : IAuthorizationFilter
{
    private const string PermissionClaimType = "permission";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // Must be authenticated first
        if (user.Identity is not { IsAuthenticated: true })
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                status  = 401,
                title   = "Unauthorized",
                message = "Authentication is required."
            });
            return;
        }

        // Check for any of the required permissions
        var hasPermission = permissions.Length == 0 || user.Claims
            .Any(c => c.Type == PermissionClaimType
                   && permissions.Contains(c.Value, StringComparer.OrdinalIgnoreCase));

        if (!hasPermission)
        {
            var reqPerms = string.Join(" or ", permissions);
            context.Result = new ObjectResult(new
            {
                status    = 403,
                title     = "Forbidden",
                message   = $"You do not have the required permission: '{reqPerms}'."
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
