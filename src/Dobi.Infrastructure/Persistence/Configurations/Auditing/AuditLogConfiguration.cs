using Dobi.Domain.Auditing;
using Dobi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Auditing
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("AuditLogId");

            builder.Property(x => x.Action)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.EntityName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.EntityId)
                .HasMaxLength(50);

            builder.Property(x => x.OldValue)
                .HasColumnType("jsonb");

            builder.Property(x => x.NewValue)
                .HasColumnType("jsonb");

            builder.Property(x => x.IpAddress)
                .HasMaxLength(50);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.Action);

            builder.HasIndex(x => x.EntityName);

            builder.HasIndex(x => x.EntityId);

            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
