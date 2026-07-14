using Dobi.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Transfers
{
    public class TransferTypeConfiguration : IEntityTypeConfiguration<TransferType>
    {
        public void Configure(EntityTypeBuilder<TransferType> builder)
        {
            builder.ToTable("TransferTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("TransferTypeId");

            builder.Property(x => x.TransferTypeCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TransferTypeName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.TransferTypeCode)
                .IsUnique();

            builder.HasIndex(x => x.TransferTypeName)
                .IsUnique();
        }
    }
}
