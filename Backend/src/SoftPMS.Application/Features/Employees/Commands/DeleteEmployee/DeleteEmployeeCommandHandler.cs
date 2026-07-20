using MediatR;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Employees.Commands.DeleteEmployee;

/// <summary>Soft-deletes the employee. The global query filter (IsDeleted == false) hides them from future queries.</summary>
public sealed class DeleteEmployeeCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await context.Employees.FindAsync([request.EmployeeId], cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Employee), request.EmployeeId);

        employee.IsDeleted = true;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
