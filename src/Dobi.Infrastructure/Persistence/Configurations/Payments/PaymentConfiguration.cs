using Dobi.Domain.Payments;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Payments
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("PaymentId");

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.PaidAt);

            builder.Property(x => x.ReceivedByUserId);

            builder.Property(x => x.ReferenceNo)
                .HasMaxLength(100);

            builder.Property(x => x.ChequeNo)
                .HasMaxLength(50);

            builder.Property(x => x.ChequeBankName)
                .HasMaxLength(150);

            builder.Property(x => x.ChequeRejectionReason)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentMethod)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentStatus)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.PaymentStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.ReceivedByUserId)
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

            builder.HasIndex(x => x.PaymentMethodId);

            builder.HasIndex(x => x.PaymentStatusId);

            builder.HasIndex(x => x.ReceivedByUserId);

            builder.HasIndex(x => x.PaidAt);

            builder.HasIndex(x => x.ReferenceNo);
        }
    }
}
