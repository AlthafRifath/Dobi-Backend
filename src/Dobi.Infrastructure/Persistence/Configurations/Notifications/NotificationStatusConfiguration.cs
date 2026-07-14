using Dobi.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Notifications
{
    public class NotificationStatusConfiguration : IEntityTypeConfiguration<NotificationStatus>
    {
        public void Configure(EntityTypeBuilder<NotificationStatus> builder)
        {
            builder.ToTable("NotificationStatuses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("NotificationStatusId");

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
