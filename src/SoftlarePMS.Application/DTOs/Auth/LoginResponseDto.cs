namespace SoftlarePMS.Application.DTOs.Auth;

public record LoginResponseDto(
    string Token,
    DateTime Expiration,
    string Username,
    string Email
);