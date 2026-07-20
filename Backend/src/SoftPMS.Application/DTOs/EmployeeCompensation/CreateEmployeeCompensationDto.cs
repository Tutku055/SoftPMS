using SoftPMS.Domain.Enums;

namespace SoftPMS.Application.DTOs.EmployeeCompensation;

public record CreateEmployeeCompensationDto(
    decimal BaseSalary,
    SalaryType SalaryType,
    string PayGrade,
    DateTime EffectiveDate
);