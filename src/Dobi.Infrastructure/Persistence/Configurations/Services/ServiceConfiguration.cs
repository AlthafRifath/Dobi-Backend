using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Services
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Dobi.Domain.Services.Service>
    {
        public void Configure(EntityTypeBuilder<Dobi.Domain.Services.Service> builder)
        {
            builder.ToTable("Services");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ServiceId");

            builder.Property(x => x.ServiceCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ServiceName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(300);

            builder.Property(x => x.IsExpressEligible)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.ServiceCode)
                .IsUnique();

            builder.HasIndex(x => x.ServiceName)
                .IsUnique();
        }
    }
}
