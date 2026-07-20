using SoftPMS.Domain.Enums;
using SoftPMS.Application.DTOs.EmployeeAddress;
using SoftPMS.Application.DTOs.EmployeeCompensation;

namespace SoftPMS.Application.DTOs.Employee;

public record CreateEmployeeDto(
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

    // Related sub-records
    List<CreateEmployeeAddressDto> Addresses,
    List<CreateEmployeeCompensationDto> Compensations
);