namespace SoftPMS.Domain.Entities;

/// <summary>
/// Immutable audit trail record written by the EF Core interceptor on every change.
/// </summary>
public class AuditLog
{
    // Long PK — audit tables are high-volume; int can overflow on large systems.
    public long Id { get; set; }

    // Name of the database table that was affected
    public string TableName { get; set; } = string.Empty;

    // Primary key of the affected row (stored as string to support Guid and int PKs)
    public string RecordId { get; set; } = string.Empty;

    // "Created", "Modified", or "Deleted"
    public string Action { get; set; } = string.Empty;

    // JSON snapshot of the entity before the change (null for Created)
    public string? OldValues { get; set; }

    // JSON snapshot of the entity after the change (null for Deleted)
    public string? NewValues { get; set; }

    // Username of the actor who triggered the change
    public string ChangedBy { get; set; } = string.Empty;

    // UTC timestamp of the change
    public DateTime ChangedAt { get; set; }
}
