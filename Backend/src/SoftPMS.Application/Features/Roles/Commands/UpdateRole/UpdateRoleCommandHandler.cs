using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<UpdateRoleCommand, Unit>
{
    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Role), request.Id);

        var nameExists = await context.Roles
            .AnyAsync(r => r.Name == request.Dto.Name && r.Id != request.Id, cancellationToken);
        
        if (nameExists)
            throw new Domain.Exceptions.DomainException($"Role '{request.Dto.Name}' already exists.");

        if (role.IsSystemRole)
            throw new Domain.Exceptions.DomainException($"System roles cannot be modified.");

        role.Name = request.Dto.Name;
        role.Description = request.Dto.Description;
        role.Color = request.Dto.Color;

        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
