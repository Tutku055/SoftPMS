using MediatR;
using SoftlarePMS.Application.DTOs.Auth;

namespace SoftlarePMS.Application.Features.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<JwtTokenResponseDto>;
