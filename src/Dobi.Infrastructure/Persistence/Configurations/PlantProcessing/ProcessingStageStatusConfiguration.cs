using Dobi.Domain.PlantProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.PlantProcessing
{
    public class ProcessingStageStatusConfiguration : IEntityTypeConfiguration<ProcessingStageStatus>
    {
        public void Configure(EntityTypeBuilder<ProcessingStageStatus> builder)
        {
            builder.ToTable("ProcessingStageStatuses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ProcessingStageStatusId");

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
