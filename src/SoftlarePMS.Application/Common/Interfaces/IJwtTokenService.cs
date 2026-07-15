using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Common.Interfaces;

/// <summary>
/// Contract for JWT token generation. Implemented in the Infrastructure layer so that
/// cryptographic dependencies stay outside the Application layer.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>Generates a signed JWT access token for the given user.</summary>
    /// <returns>Tuple of the token string and its UTC expiration timestamp.</returns>
    (string Token, DateTime Expiration) GenerateToken(User user, IEnumerable<string> roles);
}
