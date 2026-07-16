using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.DTOs.EmployeeCompensation;

public record EmployeeCompensationDto(
    Guid Id,
    decimal BaseSalary,
    SalaryType SalaryType,
    string PayGrade,
    DateTime EffectiveDate,
    bool IsActive
);