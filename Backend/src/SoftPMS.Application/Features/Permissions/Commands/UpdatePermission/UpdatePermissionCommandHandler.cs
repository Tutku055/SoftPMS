using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Permissions.Commands.UpdatePermission;

public sealed class UpdatePermissionCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<UpdatePermissionCommand, Unit>
{
    public async Task<Unit> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await context.Permissions
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Permission), request.Id);

        var nameExists = await context.Permissions
            .AnyAsync(p => p.Name == request.Dto.Name && p.Id != request.Id, cancellationToken);
        
        if (nameExists)
            throw new Domain.Exceptions.DomainException($"Permission '{request.Dto.Name}' already exists.");

        permission.Name = request.Dto.Name;
        permission.Description = request.Dto.Description;

        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
