using Dobi.Domain.Orders;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Orders
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("OrderItemId");

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.WeightKg)
                .HasPrecision(10, 2);

            builder.Property(x => x.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.LineAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.SpecialNotes)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Service)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ItemCategory)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ItemCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ServicePrice)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ServicePriceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PricingType)
                .WithMany()
                .HasForeignKey(x => x.PricingTypeId)
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

            builder.HasIndex(x => x.ServiceId);

            builder.HasIndex(x => x.ItemCategoryId);

            builder.HasIndex(x => x.ServicePriceId);

            builder.HasIndex(x => x.PricingTypeId);
        }
    }
}
