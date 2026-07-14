using Dobi.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Transfers
{
    public class TransferStatusConfiguration : IEntityTypeConfiguration<TransferStatus>
    {
        public void Configure(EntityTypeBuilder<TransferStatus> builder)
        {
            builder.ToTable("TransferStatuses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("TransferStatusId");

            builder.Property(x => x.StatusCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.StatusName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.StatusCode)
                .IsUnique();

            builder.HasIndex(x => x.StatusName)
                .IsUnique();
        }
    }
}
