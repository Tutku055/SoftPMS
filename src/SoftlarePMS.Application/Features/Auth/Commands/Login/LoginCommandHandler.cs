using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.DTOs.Auth;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Auth.Commands.Login;

/// <summary>
/// Validates credentials, generates both an access token and a refresh token,
/// persists the refresh token on the User record, and returns both to the caller.
/// </summary>
public sealed class LoginCommandHandler(
    IApplicationDbContext context,
    IJwtTokenService jwtTokenService,
    IDateTime dateTime)
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.Username);

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new DomainException("Invalid credentials.");

        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToList();

        var accessToken = jwtTokenService.GenerateAccessToken(user, permissions);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        // Persist the refresh token — tracked entity, no explicit Update() call needed
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = dateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync(cancellationToken);

        // Decode expiration from the generated token for the response
        var tokenExpiration = dateTime.UtcNow.AddMinutes(15);

        return new LoginResponseDto(accessToken, refreshToken, tokenExpiration, user.Username, user.Email);
    }
}
