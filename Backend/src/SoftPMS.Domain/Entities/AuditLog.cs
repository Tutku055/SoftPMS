namespace SoftPMS.Domain.Entities;

/// <summary>
/// Immutable audit trail record.
/// </summary>
public class AuditLog
{
    public long Id { get; set; }

    public string TableName { get; set; } = string.Empty;

    public string RecordId { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string ChangedBy { get; set; } = string.Empty;

    public DateTime ChangedAt { get; set; }
}
