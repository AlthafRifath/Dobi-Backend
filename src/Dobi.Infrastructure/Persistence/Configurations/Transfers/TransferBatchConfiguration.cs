using Dobi.Domain.Transfers;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Transfers
{
    public class TransferBatchConfiguration : IEntityTypeConfiguration<TransferBatch>
    {
        public void Configure(EntityTypeBuilder<TransferBatch> builder)
        {
            builder.ToTable("TransferBatches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("TransferBatchId");

            builder.Property(x => x.TransferNo)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.SentAt);

            builder.Property(x => x.ReceivedAt);

            builder.Property(x => x.Remarks)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.TransferType)
                .WithMany(x => x.TransferBatches)
                .HasForeignKey(x => x.TransferTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TransferStatus)
                .WithMany(x => x.TransferBatches)
                .HasForeignKey(x => x.TransferStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FromBranch)
                .WithMany()
                .HasForeignKey(x => x.FromBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FromPlant)
                .WithMany()
                .HasForeignKey(x => x.FromPlantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ToBranch)
                .WithMany()
                .HasForeignKey(x => x.ToBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ToPlant)
                .WithMany()
                .HasForeignKey(x => x.ToPlantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.DriverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TransferNo)
                .IsUnique();

            builder.HasIndex(x => x.TransferTypeId);

            builder.HasIndex(x => x.TransferStatusId);

            builder.HasIndex(x => x.DriverUserId);

            builder.HasIndex(x => x.SentAt);

            builder.HasIndex(x => x.ReceivedAt);

            builder.HasIndex(x => x.FromBranchId);

            builder.HasIndex(x => x.FromPlantId);

            builder.HasIndex(x => x.ToBranchId);

            builder.HasIndex(x => x.ToPlantId);
        }
    }
}
