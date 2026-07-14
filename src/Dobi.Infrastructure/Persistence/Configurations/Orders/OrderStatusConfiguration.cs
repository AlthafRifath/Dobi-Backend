using Dobi.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Orders
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatuses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("OrderStatusId");

            builder.Property(x => x.StatusCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.StatusName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.SortOrder)
                .IsRequired();

            builder.Property(x => x.IsTerminal)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasIndex(x => x.StatusCode)
                .IsUnique();

            builder.HasIndex(x => x.StatusName)
                .IsUnique();
        }
    }
}
