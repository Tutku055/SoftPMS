using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Roles.Commands.ToggleRoleActive;

public sealed class ToggleRoleActiveCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<ToggleRoleActiveCommand, Unit>
{
    public async Task<Unit> Handle(ToggleRoleActiveCommand request, CancellationToken cancellationToken)
    {
        var role = await context.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Role), request.Id);

        if (role.IsSystemRole)
            throw new Domain.Exceptions.DomainException("System roles cannot be deactivated.");

        // If we are deactivating, check if there are active users assigned to this role
        if (role.IsActive && role.Users.Any(u => u.IsActive && !u.IsDeleted))
            throw new Domain.Exceptions.DomainException("Cannot deactivate role because it is assigned to one or more active users.");

        role.IsActive = !role.IsActive;

        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
