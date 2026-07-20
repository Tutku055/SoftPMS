using SoftPMS.Domain.Enums;

namespace SoftPMS.Domain.Entities;

public class EmployeeDocument: BaseEntity
{
    public Guid EmployeeId { get; set; }

    public DocumentType DocumentType { get; set; } = DocumentType.Other;

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime? ReminderDate { get; set; }

    public Guid CreatedByUserId { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;

    public virtual User CreatedByUser { get; set; } = null!;
}
