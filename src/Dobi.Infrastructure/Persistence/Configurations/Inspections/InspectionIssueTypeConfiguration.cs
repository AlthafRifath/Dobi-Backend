using Dobi.Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Inspections
{
    public class InspectionIssueTypeConfiguration : IEntityTypeConfiguration<InspectionIssueType>
    {
        public void Configure(EntityTypeBuilder<InspectionIssueType> builder)
        {
            builder.ToTable("InspectionIssueTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("InspectionIssueTypeId");

            builder.Property(x => x.IssueCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.IssueName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasIndex(x => x.IssueCode)
                .IsUnique();

            builder.HasIndex(x => x.IssueName)
                .IsUnique();
        }
    }
}
