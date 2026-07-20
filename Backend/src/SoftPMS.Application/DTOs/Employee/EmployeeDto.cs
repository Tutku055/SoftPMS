using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SoftPMS.Application.DTOs.Department;
using SoftPMS.Domain.Enums;

namespace SoftPMS.Application.DTOs.Employee;

public record EmployeeDto(
    Guid Id,
    string EmployeeNo,
    string FirstName,
    string LastName,
    Gender Gender,
    EmploymentStatus EmploymentStatus,
    string Profession,
    DateTime HireDate,
    DateTime? TerminationDate,
    DateTime? ProbationEndDate,
    decimal WorkingHoursPerWeek,
    int VacationDaysTotal,
    DepartmentDto? Department
);
