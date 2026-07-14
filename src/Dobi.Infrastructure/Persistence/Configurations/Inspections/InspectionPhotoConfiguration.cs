using Dobi.Domain.Inspections;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Inspections
{
    public class InspectionPhotoConfiguration : IEntityTypeConfiguration<InspectionPhoto>
    {
        public void Configure(EntityTypeBuilder<InspectionPhoto> builder)
        {
            builder.ToTable("InspectionPhotos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("InspectionPhotoId");

            builder.Property(x => x.PhotoUrl)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.UploadedAt)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedByUserId);

            builder.HasOne(x => x.InspectionRecord)
                .WithMany(x => x.Photos)
                .HasForeignKey(x => x.InspectionRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.InspectionRecordId);

            builder.HasIndex(x => x.UploadedByUserId);

            builder.HasIndex(x => x.UploadedAt);
        }
    }
}
