using Dobi.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Services
{
    public class PricingTypeConfiguration : IEntityTypeConfiguration<PricingType>
    {
        public void Configure(EntityTypeBuilder<PricingType> builder)
        {
            builder.ToTable("PricingTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("PricingTypeId");

            builder.Property(x => x.PricingTypeCode)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.PricingTypeName)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.PricingTypeCode)
                .IsUnique();
        }
    }
}
