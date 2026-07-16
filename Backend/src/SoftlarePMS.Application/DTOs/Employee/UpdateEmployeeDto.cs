using System;
using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.DTOs.Employee;

public record UpdateEmployeeDto(
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
    int VacationDaysTotal
);
