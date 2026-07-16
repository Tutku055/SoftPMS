using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using SoftlarePMS.Application.Common.Interfaces;

namespace SoftlarePMS.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public Guid UserId
    {
        get
        {
            // The JWT middleware maps the 'sub' claim to ClaimTypes.NameIdentifier automatically.
            var value = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }
    }

    /// <summary>
    /// Returns the username. The token stores the email under the standard 'email' claim
    /// (JwtRegisteredClaimNames.Email → ClaimTypes.Email). Fall back to sub if absent.
    /// </summary>
    public string Username =>
        Principal?.FindFirstValue(ClaimTypes.Email)
        ?? Principal?.FindFirstValue(JwtRegisteredClaimNames.Email)
        ?? Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? string.Empty;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;
}
