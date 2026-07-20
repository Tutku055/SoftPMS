namespace SoftPMS.Application.DTOs.Auth;

public record LoginResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiration,
    string Username,
    string Email
);