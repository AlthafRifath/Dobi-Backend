using Dobi.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Services
{
    public class ServicePriceConfiguration : IEntityTypeConfiguration<ServicePrice>
    {
        public void Configure(EntityTypeBuilder<ServicePrice> builder)
        {
            builder.ToTable("ServicePrices");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ServicePriceId");

            builder.Property(x => x.BasePrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.ExpressAdditionalPrice)
                .HasPrecision(18, 2);

            builder.Property(x => x.EffectiveFrom)
                .IsRequired();

            builder.Property(x => x.EffectiveTo);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.Service)
                .WithMany(x => x.ServicePrices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ItemCategory)
                .WithMany(x => x.ServicePrices)
                .HasForeignKey(x => x.ItemCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PricingType)
                .WithMany(x => x.ServicePrices)
                .HasForeignKey(x => x.PricingTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.ServiceId,
                x.ItemCategoryId,
                x.PricingTypeId,
                x.EffectiveFrom
            });
        }
    }
}
