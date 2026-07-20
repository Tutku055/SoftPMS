using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Entities;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Users.Commands.AssignRolesToUser;

public sealed class AssignRolesToUserCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AssignRolesToUserCommand, Unit>
{
    public async Task<Unit> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
    {
        // Normalise: treat a null list as an empty list (removes all roles)
        var roleIds = request.RoleIds ?? [];

        var user = await context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        // Validate all requested roles exist
        var distinctRoleIds = roleIds.Distinct().ToList();

        if (distinctRoleIds.Count > 0)
        {
            var roles = await context.Roles
                .Where(r => distinctRoleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);

            if (roles.Count != distinctRoleIds.Count)
                throw new NotFoundException(nameof(Role), "one or more requested role IDs");
        }

        // Replace the full set (idempotent)
        context.UserRoles.RemoveRange(user.UserRoles);

        var newLinks = distinctRoleIds
            .Select(rid => new UserRole { UserId = user.Id, RoleId = rid });

        await context.UserRoles.AddRangeAsync(newLinks, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
