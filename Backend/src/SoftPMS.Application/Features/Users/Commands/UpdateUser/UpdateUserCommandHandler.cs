using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        var emailExists = await context.Users
            .AnyAsync(u => u.Email == request.Dto.Email && u.Id != request.Id, cancellationToken);
        
        if (emailExists)
            throw new Domain.Exceptions.DomainException($"Email '{request.Dto.Email}' is already taken.");

        user.EmployeeId = request.Dto.EmployeeId;
        user.Email = request.Dto.Email;
        user.IsActive = request.Dto.IsActive;

        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
