using System.Security.Claims;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Common.Interfaces;

/// <summary>
/// Contract for JWT token operations. Implemented in the Infrastructure layer so that
/// cryptographic dependencies stay outside the Application layer.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>Generates a signed JWT containing UserId, Email, and per-permission claims.</summary>
    string GenerateAccessToken(User user, List<string> permissions);

    /// <summary>Generates a cryptographically random, opaque refresh token string.</summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Extracts the <see cref="ClaimsPrincipal"/> from an expired token without validating lifetime.
    /// Throws <see cref="Microsoft.IdentityModel.Tokens.SecurityTokenException"/> if the token is structurally invalid.
    /// </summary>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
