using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (currentUserService.UserId == request.Id)
        {
            throw new DomainException("Users cannot delete their own accounts.");
        }
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        if (user.IsSystemUser)
        {
            throw new DomainException("System users (Super Admin) cannot be deleted.");
        }

        if (user.IsActive)
        {
            throw new DomainException("Cannot delete user because they are currently active. Please set them to inactive first.");
        }

        user.IsDeleted = true;
        user.IsActive = false;
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
