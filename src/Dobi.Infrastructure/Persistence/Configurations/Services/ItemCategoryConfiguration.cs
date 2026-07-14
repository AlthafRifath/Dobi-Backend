using Dobi.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Services
{
    public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
    {
        public void Configure(EntityTypeBuilder<ItemCategory> builder)
        {
            builder.ToTable("ItemCategories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ItemCategoryId");

            builder.Property(x => x.CategoryName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.IsSpecialItem)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.DefaultPricingType)
                .WithMany(x => x.ItemCategories)
                .HasForeignKey(x => x.DefaultPricingTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.CategoryName)
                .IsUnique();
        }
    }
}
