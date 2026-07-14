using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Persistence.Configurations;

public class EmployeeCompensationConfiguration : IEntityTypeConfiguration<EmployeeCompensation>
{
    public void Configure(EntityTypeBuilder<EmployeeCompensation> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id);

        //Money type configuration for BaseSalary
        builder.Property(c => c.BaseSalary)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        //ENUM Conversion
        builder.Property(c => c.SalaryType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(c => c.PayGrade).HasMaxLength(50);
    }
}
