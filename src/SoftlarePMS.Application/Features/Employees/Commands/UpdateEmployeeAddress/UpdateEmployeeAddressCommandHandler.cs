using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Employees.Commands.UpdateEmployeeAddress;

/// <summary>
/// Implements historic address tracking:
///   1. Locate the currently active address (EndDate == null).
///   2. Close it: set EndDate = NewStartDate - 1 day.
///   3. Insert a new address record with StartDate = NewStartDate and EndDate = null.
/// Both operations are committed in a single SaveChangesAsync call.
/// </summary>
public sealed class UpdateEmployeeAddressCommandHandler(
    IApplicationDbContext context,
    IDateTime dateTime)
    : IRequestHandler<UpdateEmployeeAddressCommand, Unit>
{
    public async Task<Unit> Handle(UpdateEmployeeAddressCommand request, CancellationToken cancellationToken)
    {
        // Verify the employee exists (respects the global IsDeleted filter)
        var employeeExists = await context.Employees
            .AnyAsync(e => e.Id == request.EmployeeId, cancellationToken);

        if (!employeeExists)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        // Step 1: Close the current active address
        var activeAddress = await context.EmployeeAddresses
            .FirstOrDefaultAsync(
                a => a.EmployeeId == request.EmployeeId && a.EndDate == null,
                cancellationToken);

        if (activeAddress is not null)
        {
            // EndDate is set to one day before the new address becomes effective
            activeAddress.EndDate = request.NewStartDate.AddDays(-1).Date;
        }

        // Step 2: Insert the new address as the active record
        var newAddress = new EmployeeAddress
        {
            EmployeeId  = request.EmployeeId,
            AddressLine = request.AddressLine,
            PostalCode  = request.PostalCode,
            City        = request.City,
            State       = request.State,
            Country     = request.Country,
            IsPrimary   = request.IsPrimary,
            StartDate   = request.NewStartDate.Date,
            EndDate     = null,
            CreatedAt   = dateTime.UtcNow
        };

        await context.EmployeeAddresses.AddAsync(newAddress, cancellationToken);

        // Both the close and the insert are committed atomically
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
