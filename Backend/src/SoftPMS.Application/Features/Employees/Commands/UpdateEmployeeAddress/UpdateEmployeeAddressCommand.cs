using MediatR;

namespace SoftPMS.Application.Features.Employees.Commands.UpdateEmployeeAddress;

/// <summary>
/// Command to update an employee's address.
/// </summary>
public sealed record UpdateEmployeeAddressCommand(
    Guid EmployeeId,
    string AddressLine,
    string PostalCode,
    string City,
    string State,
    string Country,
    bool IsPrimary
) : IRequest<Unit>;
