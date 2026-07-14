using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        // Configure the Employee entity
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmployeeNo).IsRequired().HasMaxLength(20);
        builder.HasIndex(e => e.EmployeeNo).IsUnique();

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);

        // Enum conversions for Gender and EmploymentStatus
        builder.Property(e => e.Gender)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.EmploymentStatus)
            .HasConversion<int>()
            .IsRequired();

        // Relationships with Addresses and Compensations
        builder.HasMany(e => e.Addresses)
            .WithOne(a => a.Employee)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade); // Çalışan silinirse adresi de silinsin

        builder.HasMany(e => e.Compensations)
            .WithOne(c => c.Employee)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
