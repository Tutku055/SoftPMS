using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Persistence.Configurations;

public class EmployeeNoteConfiguration : IEntityTypeConfiguration<EmployeeNote>
{
    public void Configure(EntityTypeBuilder<EmployeeNote> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        // FK to Employee — cascade delete removes notes when employee is hard-deleted
        builder.HasOne(n => n.Employee)
            .WithMany(e => e.Notes)
            .HasForeignKey(n => n.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // FK to creating User — restrict to avoid accidental user deletion cascade
        builder.HasOne(n => n.CreatedByUser)
            .WithMany(u => u.CreatedNotes)
            .HasForeignKey(n => n.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
