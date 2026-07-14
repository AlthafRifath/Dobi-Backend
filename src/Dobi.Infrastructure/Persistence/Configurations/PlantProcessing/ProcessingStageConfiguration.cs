using Dobi.Domain.PlantProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.PlantProcessing
{
    public class ProcessingStageConfiguration : IEntityTypeConfiguration<ProcessingStage>
    {
        public void Configure(EntityTypeBuilder<ProcessingStage> builder)
        {
            builder.ToTable("ProcessingStages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ProcessingStageId");

            builder.Property(x => x.StageCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.StageName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.SortOrder)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasIndex(x => x.StageCode)
                .IsUnique();

            builder.HasIndex(x => x.StageName)
                .IsUnique();

            builder.HasIndex(x => x.SortOrder);
        }
    }
}
