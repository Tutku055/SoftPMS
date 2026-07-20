using MediatR;
using SoftPMS.Domain.Enums;

namespace SoftPMS.Application.Features.Employees.Commands.CreateEmployee;

/// <summary>
/// Command to create a new employee record together with their initial address
/// and compensation entry. Returns the new employee's identity.
/// </summary>
public sealed record CreateEmployeeCommand(
    // --- Core employee fields ---
    string EmployeeNo,
    string FirstName,
    string LastName,
    Gender Gender,
    DateTime DateOfBirth,
    string Nationality,
    string Profession,
    EmploymentStatus EmploymentStatus,
    DateTime HireDate,
    decimal WorkingHoursPerWeek,
    int VacationDaysTotal,

    // --- Initial address (required at creation) ---
    string AddressLine,
    string PostalCode,
    string City,
    string State,
    string Country,

    // --- Initial compensation (Stundenlohn or monthly) ---
    decimal BaseSalary,
    SalaryType SalaryType,
    string PayGrade,

    // --- Department ---
    Guid? DepartmentId
) : IRequest<CreatedEmployeeDto>;
