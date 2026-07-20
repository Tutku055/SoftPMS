using MediatR;

namespace SoftPMS.Application.Features.Employees.Commands.DeleteEmployee;

/// <summary>Soft-deletes an employee by setting IsDeleted = true. No physical row removal.</summary>
public sealed record DeleteEmployeeCommand(Guid EmployeeId) : IRequest<Unit>;
