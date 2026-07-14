using Dobi.Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Inspections
{
    public class InspectionIssueConfiguration : IEntityTypeConfiguration<InspectionIssue>
    {
        public void Configure(EntityTypeBuilder<InspectionIssue> builder)
        {
            builder.ToTable("InspectionIssues");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("InspectionIssueId");

            builder.Property(x => x.Notes)
                .HasMaxLength(300);

            builder.HasOne(x => x.InspectionRecord)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.InspectionRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.InspectionIssueType)
                .WithMany(x => x.InspectionIssues)
                .HasForeignKey(x => x.InspectionIssueTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.InspectionRecordId);

            builder.HasIndex(x => x.InspectionIssueTypeId);

            builder.HasIndex(x => new
            {
                x.InspectionRecordId,
                x.InspectionIssueTypeId
            });
        }
    }
}
