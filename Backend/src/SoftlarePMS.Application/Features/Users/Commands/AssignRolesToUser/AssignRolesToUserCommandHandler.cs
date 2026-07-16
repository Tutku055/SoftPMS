using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Users.Commands.AssignRolesToUser;

public sealed class AssignRolesToUserCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AssignRolesToUserCommand, Unit>
{
    public async Task<Unit> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        // Validate all requested roles exist
        var roles = await context.Roles
            .Where(r => request.RoleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        if (roles.Count != request.RoleIds.Distinct().Count())
            throw new NotFoundException(nameof(Role), "one or more requested role IDs");

        // Replace the full set (idempotent)
        context.UserRoles.RemoveRange(user.UserRoles);

        var newLinks = request.RoleIds
            .Distinct()
            .Select(rid => new UserRole { UserId = user.Id, RoleId = rid });

        await context.UserRoles.AddRangeAsync(newLinks, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
