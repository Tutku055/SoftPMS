using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.Features.Employees.Queries.GetEmployeeRateHistory;

/// <summary>
/// A single entry in an employee's compensation (rate/Stundenlohn) history.
/// IsActive = true when EndDate is null (the currently effective rate).
/// </summary>
public sealed record CompensationHistoryDto(
    Guid Id,
    decimal BaseSalary,
    SalaryType SalaryType,
    string PayGrade,
    DateTime EffectiveDate,
    DateTime? EndDate,
    bool IsActive
);
