using Dobi.Domain.Payments;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Payments
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.ToTable("Refunds");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("RefundId");

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.RejectionReason)
                .HasMaxLength(500);

            builder.Property(x => x.ApprovedByUserId);

            builder.Property(x => x.RefundedAt);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Refunds)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Payment)
                .WithMany(x => x.Refunds)
                .HasForeignKey(x => x.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RefundStatus)
                .WithMany(x => x.Refunds)
                .HasForeignKey(x => x.RefundStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.ApprovedByUserId)
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

            builder.HasIndex(x => x.PaymentId);

            builder.HasIndex(x => x.RefundStatusId);

            builder.HasIndex(x => x.ApprovedByUserId);

            builder.HasIndex(x => x.RefundedAt);
        }
    }
}
