using Dobi.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Notifications
{
    public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
    {
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            builder.ToTable("NotificationTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("NotificationTypeId");

            builder.Property(x => x.TypeCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TypeName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.TypeCode)
                .IsUnique();

            builder.HasIndex(x => x.TypeName)
                .IsUnique();
        }
    }
}
