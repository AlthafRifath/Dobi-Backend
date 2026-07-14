using Dobi.Domain.Inspections;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Inspections
{
    public class InspectionRecordConfiguration : IEntityTypeConfiguration<InspectionRecord>
    {
        public void Configure(EntityTypeBuilder<InspectionRecord> builder)
        {
            builder.ToTable("InspectionRecords");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("InspectionRecordId");

            builder.Property(x => x.CustomerAcknowledged)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.CustomerSignatureUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.InspectedAt)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.InspectionRecords)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OrderItem)
                .WithMany(x => x.InspectionRecords)
                .HasForeignKey(x => x.OrderItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.InspectedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OrderId);

            builder.HasIndex(x => x.OrderItemId);

            builder.HasIndex(x => x.InspectedByUserId);

            builder.HasIndex(x => x.InspectedAt);
        }
    }
}
