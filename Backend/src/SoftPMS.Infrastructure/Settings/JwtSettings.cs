using System.ComponentModel.DataAnnotations;

namespace SoftPMS.Infrastructure.Settings;

public sealed class JwtSettings
{
    [Required, MinLength(32)]
    public string SecretKey { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Range(1, 1440)]
    public int AccessTokenExpirationInMinutes { get; init; } = 15;

    [Range(1, 365)]
    public int RefreshTokenExpirationInDays { get; init; } = 7;
}
