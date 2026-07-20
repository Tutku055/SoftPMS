using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Entities;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Departments.Commands.DeleteDepartment;

public sealed class DeleteDepartmentCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<DeleteDepartmentCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var hasEmployees = await context.Employees
            .AnyAsync(e => e.DepartmentId == request.Id, cancellationToken);

        if (hasEmployees)
        {
            throw new DomainException("Cannot delete department because it has one or more employees assigned to it.");
        }

        var department = await context.Departments
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (department is null)
        {
            throw new NotFoundException(nameof(Department), request.Id);
        }

        context.Departments.Remove(department);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
