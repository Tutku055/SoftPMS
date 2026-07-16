using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
