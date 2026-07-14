using Dobi.Domain.Transfers;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Transfers
{
    public class TransferAcknowledgementConfiguration : IEntityTypeConfiguration<TransferAcknowledgement>
    {
        public void Configure(EntityTypeBuilder<TransferAcknowledgement> builder)
        {
            builder.ToTable("TransferAcknowledgements");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("TransferAcknowledgementId");

            builder.Property(x => x.AcknowledgedAt)
                .IsRequired();

            builder.Property(x => x.SignatureUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Remarks)
                .HasMaxLength(300);

            builder.HasOne(x => x.TransferBatch)
                .WithMany(x => x.Acknowledgements)
                .HasForeignKey(x => x.TransferBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AcknowledgementType)
                .WithMany(x => x.TransferAcknowledgements)
                .HasForeignKey(x => x.AcknowledgementTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TransferBatchId);

            builder.HasIndex(x => x.AcknowledgementTypeId);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.AcknowledgedAt);

            builder.HasIndex(x => new
            {
                x.TransferBatchId,
                x.AcknowledgementTypeId
            });
        }
    }
}
