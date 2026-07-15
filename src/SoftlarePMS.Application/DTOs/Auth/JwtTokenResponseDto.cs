namespace SoftlarePMS.Application.DTOs.Auth;

public record JwtTokenResponseDto(
    string AccessToken,
    string RefreshToken
);
