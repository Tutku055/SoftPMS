using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Permissions.Commands.DeletePermission;

public sealed class DeletePermissionCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<DeletePermissionCommand, Unit>
{
    public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await context.Permissions
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Permission), request.Id);

        context.Permissions.Remove(permission);
        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
