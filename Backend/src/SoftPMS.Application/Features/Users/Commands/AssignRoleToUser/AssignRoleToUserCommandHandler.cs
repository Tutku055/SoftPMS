using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Entities;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Users.Commands.AssignRoleToUser;

public sealed class AssignRoleToUserCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<AssignRoleToUserCommand, Unit>
{
    public async Task<Unit> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == currentUserService.UserId)
            throw new DomainException("Users cannot change their own roles.");

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var role = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken)
            ?? throw new NotFoundException(nameof(Role), request.RoleId);

        if (role.Name == "SuperAdmin")
            throw new DomainException("The Super Admin role cannot be assigned.");

        user.RoleId = request.RoleId;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
