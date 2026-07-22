using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<ChangePasswordCommand, Unit>
{
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (currentUserService.UserId != request.UserId)
        {
            throw new DomainException("You can only change your own password.");
        }

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.UserId);

        if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
        {
            throw new DomainException("Invalid old password.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.RequiresPasswordChange = false;

        if (user.IsSystemUser && user.RoleId != Guid.Empty)
        {
            var allPermissionIds = await context.Permissions
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            var existingRolePermissions = await context.RolePermissions
                .Where(rp => rp.RoleId == user.RoleId)
                .Select(rp => rp.PermissionId)
                .ToListAsync(cancellationToken);

            var missingPermissionIds = allPermissionIds.Except(existingRolePermissions).ToList();
            
            if (missingPermissionIds.Any())
            {
                context.RolePermissions.AddRange(missingPermissionIds.Select(pid => new Domain.Entities.RolePermission
                {
                    RoleId = user.RoleId,
                    PermissionId = pid
                }));
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
