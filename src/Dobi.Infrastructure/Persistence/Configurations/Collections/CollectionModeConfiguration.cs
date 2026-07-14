using Dobi.Domain.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.Collections
{
    public class CollectionModeConfiguration : IEntityTypeConfiguration<CollectionMode>
    {
        public void Configure(EntityTypeBuilder<CollectionMode> builder)
        {
            builder.ToTable("CollectionModes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CollectionModeId");

            builder.Property(x => x.CollectionModeCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CollectionModeName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.CollectionModeCode)
                .IsUnique();

            builder.HasIndex(x => x.CollectionModeName)
                .IsUnique();
        }
    }
}
