using Dobi.Domain.Identity;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Identity
{
    public class UserPlantAssignmentConfiguration : IEntityTypeConfiguration<UserPlantAssignment>
    {
        public void Configure(EntityTypeBuilder<UserPlantAssignment> builder)
        {
            builder.ToTable("UserPlantAssignments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("UserPlantAssignmentId");

            builder.Property(x => x.IsPrimary)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany(x => x.PlantAssignments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Plant)
                .WithMany(x => x.UserAssignments)
                .HasForeignKey(x => x.PlantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.UserId, x.PlantId })
                .IsUnique();
        }
    }
}
