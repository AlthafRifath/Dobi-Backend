using Dobi.Domain.Orders;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Orders
{
    public class OrderItemTagConfiguration : IEntityTypeConfiguration<OrderItemTag>
    {
        public void Configure(EntityTypeBuilder<OrderItemTag> builder)
        {
            builder.ToTable("OrderItemTags");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("OrderItemTagId");

            builder.Property(x => x.TagNo)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PieceNo);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.OrderItem)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TagNo)
                .IsUnique();

            builder.HasIndex(x => x.OrderItemId);
        }
    }
}
