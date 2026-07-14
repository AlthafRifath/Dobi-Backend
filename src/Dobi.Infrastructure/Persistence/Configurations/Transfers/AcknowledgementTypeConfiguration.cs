using Dobi.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Transfers
{
    public class AcknowledgementTypeConfiguration : IEntityTypeConfiguration<AcknowledgementType>
    {
        public void Configure(EntityTypeBuilder<AcknowledgementType> builder)
        {
            builder.ToTable("AcknowledgementTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("AcknowledgementTypeId");

            builder.Property(x => x.AcknowledgementCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.AcknowledgementName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.AcknowledgementCode)
                .IsUnique();

            builder.HasIndex(x => x.AcknowledgementName)
                .IsUnique();
        }
    }
}
