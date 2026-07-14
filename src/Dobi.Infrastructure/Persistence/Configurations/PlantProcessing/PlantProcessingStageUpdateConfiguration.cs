using Dobi.Domain.PlantProcessing;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.PlantProcessing
{
    public class PlantProcessingStageUpdateConfiguration : IEntityTypeConfiguration<PlantProcessingStageUpdate>
    {
        public void Configure(EntityTypeBuilder<PlantProcessingStageUpdate> builder)
        {
            builder.ToTable("PlantProcessingStageUpdates");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("PlantProcessingStageUpdateId");

            builder.Property(x => x.StartedAt);

            builder.Property(x => x.CompletedAt);

            builder.Property(x => x.Remarks)
                .HasMaxLength(500);

            builder.HasOne(x => x.PlantProcessing)
                .WithMany(x => x.StageUpdates)
                .HasForeignKey(x => x.PlantProcessingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProcessingStage)
                .WithMany(x => x.StageUpdates)
                .HasForeignKey(x => x.ProcessingStageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProcessingStageStatus)
                .WithMany(x => x.StageUpdates)
                .HasForeignKey(x => x.ProcessingStageStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PlantProcessingId);

            builder.HasIndex(x => x.ProcessingStageId);

            builder.HasIndex(x => x.ProcessingStageStatusId);

            builder.HasIndex(x => x.UpdatedByUserId);

            builder.HasIndex(x => x.StartedAt);

            builder.HasIndex(x => x.CompletedAt);

            builder.HasIndex(x => new
            {
                x.PlantProcessingId,
                x.ProcessingStageId
            });
        }
    }
}
