using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Domain.Exceptions;

namespace SoftPMS.Application.Features.Employees.Queries.GetEmployeeRateHistory;

/// <summary>
/// Returns all compensation records for the given employee ordered newest-first.
/// The active record (EndDate == null) is flagged as IsActive = true.
/// Designed to feed frontend salary/rate history charts.
/// </summary>
public sealed class GetEmployeeRateHistoryQueryHandler(
    IApplicationDbContext context)
    : IRequestHandler<GetEmployeeRateHistoryQuery, IReadOnlyList<CompensationHistoryDto>>
{
    public async Task<IReadOnlyList<CompensationHistoryDto>> Handle(
        GetEmployeeRateHistoryQuery request,
        CancellationToken cancellationToken)
    {
        // Guard: ensure the employee exists
        var employeeExists = await context.Employees
            .AnyAsync(e => e.Id == request.EmployeeId, cancellationToken);

        if (!employeeExists)
            throw new NotFoundException(nameof(Domain.Entities.Employee), request.EmployeeId);

        // Project directly in the database — no entity materialisation needed
        var history = await context.EmployeeCompensations
            .Where(c => c.EmployeeId == request.EmployeeId)
            .OrderByDescending(c => c.EffectiveDate)
            .Select(c => new CompensationHistoryDto(
                c.Id,
                c.BaseSalary,
                c.SalaryType,
                c.PayGrade,
                c.EffectiveDate,
                c.EndDate,
                c.EndDate == null   // active when EndDate is null
            ))
            .ToListAsync(cancellationToken);

        return history;
    }
}
