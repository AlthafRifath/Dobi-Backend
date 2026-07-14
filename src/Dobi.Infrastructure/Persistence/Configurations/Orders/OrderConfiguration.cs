using Dobi.Domain.Orders;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("OrderId");

            builder.Property(x => x.OrderNo)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.OrderDate)
                .IsRequired();

            builder.Property(x => x.ExpectedReturnDate);

            builder.Property(x => x.IsExpress)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.SubTotalAmount)
                .HasPrecision(18, 2)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.ExpressChargeAmount)
                .HasPrecision(18, 2)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.TotalAmount)
                .HasPrecision(18, 2)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CurrentStatus)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CurrentStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentStatus)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.PaymentStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OrderNo)
                .IsUnique();

            builder.HasIndex(x => x.CustomerId);

            builder.HasIndex(x => x.BranchId);

            builder.HasIndex(x => x.CurrentStatusId);

            builder.HasIndex(x => x.PaymentStatusId);

            builder.HasIndex(x => x.OrderDate);

            builder.HasIndex(x => x.ExpectedReturnDate);
        }
    }
}
