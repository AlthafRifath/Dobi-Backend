using Dobi.Domain.Collections;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Collections
{
    public class OrderCollectionConfiguration : IEntityTypeConfiguration<OrderCollection>
    {
        public void Configure(EntityTypeBuilder<OrderCollection> builder)
        {
            builder.ToTable("OrderCollections");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("OrderCollectionId");

            builder.Property(x => x.IsCollectedByCustomer)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CollectorName)
                .HasMaxLength(150);

            builder.Property(x => x.CollectorMobileNo)
                .HasMaxLength(20);

            builder.Property(x => x.ReceiptVerified)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.MobileNoVerified)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.ReceiptImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.CustomerSignatureUrl)
                .HasMaxLength(500);

            builder.Property(x => x.CollectedOrDeliveredAt)
                .IsRequired();

            builder.Property(x => x.Remarks)
                .HasMaxLength(500);

            builder.HasOne(x => x.Order)
                .WithOne(x => x.Collection)
                .HasForeignKey<OrderCollection>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CollectionMode)
                .WithMany(x => x.OrderCollections)
                .HasForeignKey(x => x.CollectionModeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.ReleasedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OrderId)
                .IsUnique();

            builder.HasIndex(x => x.CollectionModeId);

            builder.HasIndex(x => x.ReleasedByUserId);

            builder.HasIndex(x => x.CollectedOrDeliveredAt);

            builder.HasIndex(x => x.CollectorMobileNo);
        }
    }
}
