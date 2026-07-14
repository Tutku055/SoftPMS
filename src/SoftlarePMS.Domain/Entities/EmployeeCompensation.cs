using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Domain.Entities;

public class EmployeeCompensation: BaseEntity
{
    public Guid EmployeeId { get; set; }

    public string PayGrade { get; set; } = string.Empty;

    public decimal BaseSalary { get; set; }

    public SalaryType SalaryType { get; set; } = SalaryType.Monthly;

    public DateTime EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int CreatedByUserId { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;

    public virtual User CreatedByUser { get; set; } = null!;
}
