using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Employees.Commands.UpdateEmployeeRate;

/// <summary>
/// Implements historic rate (Stundenlohn) tracking:
///   1. Locate the currently active compensation record (EndDate == null).
///   2. Close it: set EndDate = NewEffectiveDate - 1 day.
///   3. Insert a new compensation record with EffectiveDate = NewEffectiveDate and EndDate = null.
/// Both operations are committed in a single SaveChangesAsync call.
/// </summary>
public sealed class UpdateEmployeeRateCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTime dateTime)
    : IRequestHandler<UpdateEmployeeRateCommand, Unit>
{
    public async Task<Unit> Handle(UpdateEmployeeRateCommand request, CancellationToken cancellationToken)
    {
        // Verify employee exists
        var employeeExists = await context.Employees
            .AnyAsync(e => e.Id == request.EmployeeId, cancellationToken);

        if (!employeeExists)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        // Step 1: Close the current active compensation record
        var activeCompensation = await context.EmployeeCompensations
            .FirstOrDefaultAsync(
                c => c.EmployeeId == request.EmployeeId && c.EndDate == null,
                cancellationToken);

        if (activeCompensation is not null)
        {
            // EndDate is one day before the new rate becomes effective
            activeCompensation.EndDate = request.NewEffectiveDate.AddDays(-1).Date;
        }

        // Step 2: Insert new active compensation record
        var newCompensation = new EmployeeCompensation
        {
            EmployeeId      = request.EmployeeId,
            BaseSalary      = request.BaseSalary,
            SalaryType      = request.SalaryType,
            PayGrade        = request.PayGrade,
            EffectiveDate   = request.NewEffectiveDate.Date,
            EndDate         = null,
            CreatedByUserId = currentUser.UserId,
            CreatedAt       = dateTime.UtcNow
        };

        await context.EmployeeCompensations.AddAsync(newCompensation, cancellationToken);

        // Commit the close + insert atomically
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
