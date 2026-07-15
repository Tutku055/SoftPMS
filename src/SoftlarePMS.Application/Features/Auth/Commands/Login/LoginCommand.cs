using MediatR;
using SoftlarePMS.Application.DTOs.Auth;

namespace SoftlarePMS.Application.Features.Auth.Commands.Login;

/// <summary>Command to authenticate a user and receive a JWT access token.</summary>
public sealed record LoginCommand(
    string Username,
    string Password
) : IRequest<LoginResponseDto>;
