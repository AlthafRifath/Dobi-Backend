using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.PlantProcessing
{
    public class PlantProcessingConfiguration : IEntityTypeConfiguration<Dobi.Domain.PlantProcessing.PlantProcessing>
    {
        public void Configure(EntityTypeBuilder<Dobi.Domain.PlantProcessing.PlantProcessing> builder)
        {
            builder.ToTable("PlantProcessings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("PlantProcessingId");

            builder.Property(x => x.ReceivedAtPlant);

            builder.Property(x => x.ReadyDate);

            builder.Property(x => x.PlantRemarks)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.Order)
                .WithOne(x => x.PlantProcessing)
                .HasForeignKey<Dobi.Domain.PlantProcessing.PlantProcessing>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Plant)
                .WithMany(x => x.PlantProcessings)
                .HasForeignKey(x => x.PlantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OverallQCStatus)
                .WithMany(x => x.PlantProcessings)
                .HasForeignKey(x => x.OverallQCStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OrderId)
                .IsUnique();

            builder.HasIndex(x => x.PlantId);

            builder.HasIndex(x => x.OverallQCStatusId);

            builder.HasIndex(x => x.ReceivedAtPlant);

            builder.HasIndex(x => x.ReadyDate);
        }
    }
}
