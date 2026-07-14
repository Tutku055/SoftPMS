using SoftlarePMS.Domain.Enums;
using SoftlarePMS.Application.DTOs.EmployeeAddress;
using SoftlarePMS.Application.DTOs.EmployeeCompensation;

namespace SoftlarePMS.Application.DTOs.Employee;

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