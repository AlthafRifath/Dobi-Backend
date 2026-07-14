using Dobi.Domain.Identity;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Identity
{
    public class UserBranchAssignmentConfiguration : IEntityTypeConfiguration<UserBranchAssignment>
    {
        public void Configure(EntityTypeBuilder<UserBranchAssignment> builder)
        {
            builder.ToTable("UserBranchAssignments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("UserBranchAssignmentId");

            builder.Property(x => x.IsPrimary)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany(x => x.BranchAssignments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                .WithMany(x => x.UserAssignments)
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.UserId, x.BranchId })
                .IsUnique();
        }
    }
}
