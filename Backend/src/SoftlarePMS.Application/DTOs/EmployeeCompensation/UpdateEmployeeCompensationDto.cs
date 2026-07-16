using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.DTOs.EmployeeCompensation;

public record UpdateEmployeeCompensationDto(
    decimal BaseSalary,
    SalaryType SalaryType,
    string PayGrade,
    DateTime EffectiveDate,
    DateTime? EndDate
);
