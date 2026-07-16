using MediatR;

namespace SoftlarePMS.Application.Features.Employees.Commands.UpdateEmployeeAddress;

/// <summary>
/// Command to set a new active address for an employee.
/// The current active address (EndDate == null) will be closed at NewStartDate - 1 day.
/// A new address record is inserted with EndDate = null (active).
/// </summary>
public sealed record UpdateEmployeeAddressCommand(
    Guid EmployeeId,
    string AddressLine,
    string PostalCode,
    string City,
    string State,
    string Country,
    bool IsPrimary,
    DateTime NewStartDate
) : IRequest<Unit>;
