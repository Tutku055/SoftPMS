using MediatR;
using SoftPMS.Application.DTOs.Auth;

namespace SoftPMS.Application.Features.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<JwtTokenResponseDto>;
