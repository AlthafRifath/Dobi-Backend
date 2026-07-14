using Dobi.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Customers
{
    public class CustomerTypeConfiguration : IEntityTypeConfiguration<CustomerType>
    {
        public void Configure(EntityTypeBuilder<CustomerType> builder)
        {
            builder.ToTable("CustomerTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CustomerTypeId");

            builder.Property(x => x.CustomerTypeCode)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.CustomerTypeName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.CustomerTypeCode)
                .IsUnique();
        }
    }
}
