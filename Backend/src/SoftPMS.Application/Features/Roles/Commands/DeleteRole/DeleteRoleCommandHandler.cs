using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<DeleteRoleCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await context.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Role), request.Id);

        if (role.IsSystemRole)
            throw new Domain.Exceptions.DomainException("System roles cannot be deleted.");

        if (role.Users.Any(u => !u.IsDeleted))
            throw new Domain.Exceptions.DomainException("Cannot delete role because it is assigned to one or more active users.");

        context.Roles.Remove(role);
        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
