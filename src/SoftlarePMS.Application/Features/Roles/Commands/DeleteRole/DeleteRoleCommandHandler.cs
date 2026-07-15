using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<DeleteRoleCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Role), request.Id);

        context.Roles.Remove(role);
        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
