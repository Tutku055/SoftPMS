using SoftPMS.Domain.Enums;

namespace SoftPMS.Application.DTOs.EmployeeCompensation;

public record UpdateEmployeeCompensationDto(
    decimal BaseSalary,
    SalaryType SalaryType,
    string PayGrade,
    DateTime EffectiveDate,
    DateTime? EndDate
);
