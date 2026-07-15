using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Persistence.Configurations;

public class EmployeeAddressConfiguration : IEntityTypeConfiguration<EmployeeAddress>
{
    public void Configure(EntityTypeBuilder<EmployeeAddress> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AddressLine)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.State)
            .HasMaxLength(100);

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.PostalCode)
            .HasMaxLength(20);

        // StartDate is required — every historic record must know when it became active
        builder.Property(a => a.StartDate)
            .IsRequired();

        // EndDate is null for the currently active address
        builder.Property(a => a.EndDate)
            .IsRequired(false);

        // Composite index on (EmployeeId, EndDate) for fast "active address" lookups
        // (WHERE EndDate IS NULL is a common hot path)
        builder.HasIndex(a => new { a.EmployeeId, a.EndDate })
            .HasDatabaseName("IX_EmployeeAddresses_EmployeeId_EndDate");
    }
}
