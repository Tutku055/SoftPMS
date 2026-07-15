using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Persistence.Interceptors;

/// <summary>
/// EF Core SaveChanges interceptor that writes an AuditLog entry for every
/// Created, Modified, or Deleted entity before persisting to the database.
/// Uses ISaveChangesInterceptor exclusively — no DbContext.SaveChangesAsync override required.
/// </summary>
public sealed class AuditSaveChangesInterceptor(ICurrentUserService currentUser) : SaveChangesInterceptor
{
    // JSON serializer options — write indented for readability in the audit table
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            AddAuditEntries(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
            AddAuditEntries(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    private void AddAuditEntries(DbContext context)
    {
        var changedAt = DateTime.UtcNow;
        var changedBy = currentUser.IsAuthenticated ? currentUser.Username : "System";

        var auditEntries = context.ChangeTracker
            .Entries()
            .Where(e =>
                // Skip AuditLog itself to prevent recursive loops
                e.Entity is not AuditLog &&
                e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Select(entry =>
            {
                var action = entry.State switch
                {
                    EntityState.Added    => "Created",
                    EntityState.Modified => "Modified",
                    EntityState.Deleted  => "Deleted",
                    _                   => "Unknown"
                };

                var tableName  = entry.Metadata.GetTableName() ?? entry.Metadata.Name;
                var recordId   = GetPrimaryKeyValue(entry);
                var oldValues  = GetPropertySnapshot(entry, isOld: true);
                var newValues  = GetPropertySnapshot(entry, isOld: false);

                return new AuditLog
                {
                    TableName  = tableName,
                    RecordId   = recordId,
                    Action     = action,
                    OldValues  = oldValues,
                    NewValues  = newValues,
                    ChangedBy  = changedBy,
                    ChangedAt  = changedAt
                };
            })
            .ToList();

        if (auditEntries.Count > 0)
            context.Set<AuditLog>().AddRange(auditEntries);
    }

    /// <summary>Serialises the primary key value(s) to a compact string.</summary>
    private static string GetPrimaryKeyValue(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
    {
        var keyValues = entry.Metadata
            .FindPrimaryKey()
            ?.Properties
            .Select(p => entry.Property(p.Name).CurrentValue?.ToString() ?? "null")
            .ToArray();

        return keyValues is { Length: > 0 }
            ? string.Join(",", keyValues)
            : string.Empty;
    }

    /// <summary>
    /// Returns a JSON snapshot of the entity's current or original property values.
    /// For Added entities, OldValues is null. For Deleted entities, NewValues is null.
    /// </summary>
    private static string? GetPropertySnapshot(
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry,
        bool isOld)
    {
        // Added entities have no "before" values
        if (isOld && entry.State == EntityState.Added)  return null;
        // Deleted entities have no "after" values
        if (!isOld && entry.State == EntityState.Deleted) return null;

        var values = new Dictionary<string, object?>();

        foreach (var property in entry.Properties)
        {
            // Skip navigation shadow properties
            if (property.Metadata.IsShadowProperty()) continue;

            var value = isOld
                ? property.OriginalValue
                : property.CurrentValue;

            values[property.Metadata.Name] = value;
        }

        return JsonSerializer.Serialize(values, JsonOptions);
    }
}
