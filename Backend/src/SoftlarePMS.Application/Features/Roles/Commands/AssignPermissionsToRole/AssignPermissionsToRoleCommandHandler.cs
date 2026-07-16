using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Roles.Commands.AssignPermissionsToRole;

public sealed class AssignPermissionsToRoleCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AssignPermissionsToRoleCommand, Unit>
{
    public async Task<Unit> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
    {
        // Normalise: treat a null list as an empty list (removes all permissions)
        var permissionIds = request.PermissionIds ?? [];

        var role = await context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken)
            ?? throw new NotFoundException(nameof(Role), request.RoleId);

        // Validate all requested permissions exist
        var distinctPermissionIds = permissionIds.Distinct().ToList();

        if (distinctPermissionIds.Count > 0)
        {
            var permissions = await context.Permissions
                .Where(p => distinctPermissionIds.Contains(p.Id))
                .ToListAsync(cancellationToken);

            if (permissions.Count != distinctPermissionIds.Count)
                throw new NotFoundException(nameof(Permission), "one or more requested permission IDs");
        }

        // Replace the full set (idempotent)
        context.RolePermissions.RemoveRange(role.RolePermissions);

        var newLinks = distinctPermissionIds
            .Select(pid => new RolePermission { RoleId = role.Id, PermissionId = pid });

        await context.RolePermissions.AddRangeAsync(newLinks, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
