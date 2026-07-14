using Dobi.Domain.PlantProcessing;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.PlantProcessing
{
    public class QCRecordConfiguration : IEntityTypeConfiguration<QCRecord>
    {
        public void Configure(EntityTypeBuilder<QCRecord> builder)
        {
            builder.ToTable("QCRecords");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("QCRecordId");

            builder.Property(x => x.IssueDescription)
                .HasMaxLength(500);

            builder.Property(x => x.ActionTaken)
                .HasMaxLength(500);

            builder.Property(x => x.LabourChargeAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.RecordedAt)
                .IsRequired();

            builder.HasOne(x => x.PlantProcessing)
                .WithMany(x => x.QCRecords)
                .HasForeignKey(x => x.PlantProcessingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.OrderItem)
                .WithMany(x => x.QCRecords)
                .HasForeignKey(x => x.OrderItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.QCStatus)
                .WithMany(x => x.QCRecords)
                .HasForeignKey(x => x.QCStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.RecordedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PlantProcessingId);

            builder.HasIndex(x => x.OrderItemId);

            builder.HasIndex(x => x.QCStatusId);

            builder.HasIndex(x => x.RecordedByUserId);

            builder.HasIndex(x => x.RecordedAt);
        }
    }
}
