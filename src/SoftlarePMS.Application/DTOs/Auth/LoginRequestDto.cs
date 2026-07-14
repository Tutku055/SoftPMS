namespace SoftlarePMS.Application.DTOs.Auth;

public record LoginRequestDto(
    string Username,
    string Password
);