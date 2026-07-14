using Dobi.Domain.PlantProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Configurations.PlantProcessing
{
    public class QCStatusConfiguration : IEntityTypeConfiguration<QCStatus>
    {
        public void Configure(EntityTypeBuilder<QCStatus> builder)
        {
            builder.ToTable("QCStatuses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("QCStatusId");

            builder.Property(x => x.QCStatusCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.QCStatusName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.QCStatusCode)
                .IsUnique();

            builder.HasIndex(x => x.QCStatusName)
                .IsUnique();
        }
    }
}
