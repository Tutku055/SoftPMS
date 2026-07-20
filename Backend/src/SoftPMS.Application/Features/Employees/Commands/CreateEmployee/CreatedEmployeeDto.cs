namespace SoftPMS.Application.Features.Employees.Commands.CreateEmployee;

/// <summary>Response returned after a successful CreateEmployeeCommand.</summary>
public sealed record CreatedEmployeeDto(
    Guid Id,
    string EmployeeNo,
    DateTime HireDate
);
