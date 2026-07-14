namespace SoftlarePMS.Domain.Entities;

public class EmployeeNote: BaseEntity
{
    public Guid EmployeeId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int CreatedByUserId { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;

    public virtual User CreatedByUser { get; set; } = null!;
}
