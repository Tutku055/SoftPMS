using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Entities;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Departments.Commands.UpdateDepartment;

public sealed class UpdateDepartmentCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<UpdateDepartmentCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await context.Departments
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (department is null)
        {
            throw new NotFoundException(nameof(Department), request.Id);
        }

        department.Name = request.Name;
        department.Description = request.Description;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
