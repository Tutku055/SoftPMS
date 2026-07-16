using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(a => a.Id);

        // Use database-generated identity for the high-volume surrogate key
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.TableName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.RecordId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(20);

        // OldValues and NewValues hold JSON snapshots — use nvarchar(max)
        builder.Property(a => a.OldValues)
            .HasColumnType("nvarchar(max)");

        builder.Property(a => a.NewValues)
            .HasColumnType("nvarchar(max)");

        builder.Property(a => a.ChangedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ChangedAt)
            .IsRequired();

        // Index for querying the history of a specific record by table + PK
        builder.HasIndex(a => new { a.TableName, a.RecordId })
            .HasDatabaseName("IX_AuditLogs_TableName_RecordId");

        // Index for queries like "show all changes made by user X"
        builder.HasIndex(a => a.ChangedBy)
            .HasDatabaseName("IX_AuditLogs_ChangedBy");
    }
}
