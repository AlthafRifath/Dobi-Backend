using Dobi.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Payments
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethods");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("PaymentMethodId");

            builder.Property(x => x.MethodCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.MethodName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.MethodCode)
                .IsUnique();

            builder.HasIndex(x => x.MethodName)
                .IsUnique();
        }
    }
}
