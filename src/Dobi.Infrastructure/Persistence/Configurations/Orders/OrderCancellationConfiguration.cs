using Dobi.Domain.Orders;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Orders
{
    public class OrderCancellationConfiguration : IEntityTypeConfiguration<OrderCancellation>
    {
        public void Configure(EntityTypeBuilder<OrderCancellation> builder)
        {
            builder.ToTable("OrderCancellations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("OrderCancellationId");

            builder.Property(x => x.CancellationReason)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.RequestedByCustomer)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CancelledAt)
                .IsRequired();

            builder.HasOne(x => x.Order)
                .WithOne(x => x.Cancellation)
                .HasForeignKey<OrderCancellation>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.CancelledByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OrderId)
                .IsUnique();

            builder.HasIndex(x => x.CancelledByUserId);

            builder.HasIndex(x => x.CancelledAt);
        }
    }
}
