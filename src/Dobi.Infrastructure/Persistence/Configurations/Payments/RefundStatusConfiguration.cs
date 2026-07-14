using Dobi.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Payments
{
    public class RefundStatusConfiguration : IEntityTypeConfiguration<RefundStatus>
    {
        public void Configure(EntityTypeBuilder<RefundStatus> builder)
        {
            builder.ToTable("RefundStatuses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("RefundStatusId");

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
