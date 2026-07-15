using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Persistence.Configurations;

public class EmployeeDocumentConfiguration : IEntityTypeConfiguration<EmployeeDocument>
{
    public void Configure(EntityTypeBuilder<EmployeeDocument> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.DocumentType)
            .HasConversion<int>()
            .IsRequired();

        // FK to creating User — restrict to prevent cascade on user removal
        builder.HasOne(d => d.CreatedByUser)
            .WithMany(u => u.CreatedDocuments)
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
