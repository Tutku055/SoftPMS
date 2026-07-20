using MediatR;
using SoftPMS.Application.DTOs.Auth;

namespace SoftPMS.Application.Features.Auth.Commands.Login;

/// <summary>Command to authenticate a user and receive a JWT access token.</summary>
public sealed record LoginCommand(
    string Username,
    string Password
) : IRequest<LoginResponseDto>;
