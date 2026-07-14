using Dobi.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Transfers
{
    public class TransferBatchItemConfiguration : IEntityTypeConfiguration<TransferBatchItem>
    {
        public void Configure(EntityTypeBuilder<TransferBatchItem> builder)
        {
            builder.ToTable("TransferBatchItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("TransferBatchItemId");

            builder.Property(x => x.NoOfBags)
                .IsRequired();

            builder.Property(x => x.NoOfPieces)
                .IsRequired();

            builder.Property(x => x.Remarks)
                .HasMaxLength(300);

            builder.HasOne(x => x.TransferBatch)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.TransferBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.TransferBatchItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TransferBatchId);

            builder.HasIndex(x => x.OrderId);

            builder.HasIndex(x => new
            {
                x.TransferBatchId,
                x.OrderId
            }).IsUnique();
        }
    }
}
