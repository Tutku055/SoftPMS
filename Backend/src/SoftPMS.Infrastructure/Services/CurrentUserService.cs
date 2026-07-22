using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using SoftPMS.Application.Common.Interfaces;

namespace SoftPMS.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public Guid UserId
    {
        get
        {
            var value = Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? Principal?.FindFirstValue("sub");
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
