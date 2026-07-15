using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.DTOs.Auth;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Auth.Commands.Login;

/// <summary>
/// Validates credentials against the database, then delegates token generation to
/// IJwtTokenService (implemented in the Infrastructure layer).
/// Throws Domain.Exceptions.NotFoundException when the user does not exist or is inactive.
/// Throws Domain.Exceptions.DomainException when the password is incorrect.
/// </summary>
public sealed class LoginCommandHandler(
    IApplicationDbContext context,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Fetch user with roles in a single query
        var user = await context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.Username);

        // Verify password hash — BCrypt.Verify is the recommended pattern; we guard via DomainException
        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!passwordValid)
            throw new DomainException("Invalid credentials.");

        // Resolve role names for the JWT claims
        var roleNames = user.UserRoles
            .Select(ur => ur.Role?.Name ?? string.Empty)
            .Where(r => !string.IsNullOrEmpty(r))
            .ToList();

        var (token, expiration) = jwtTokenService.GenerateToken(user, roleNames);

        return new LoginResponseDto(token, expiration, user.Username, user.Email);
    }
}
