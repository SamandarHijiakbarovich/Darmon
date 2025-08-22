using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class ClickTransactionConfiguration: IEntityTypeConfiguration<ClickTransaction>
{
    public void Configure(EntityTypeBuilder<ClickTransaction> entity)
    {
        // Primary key
        entity.HasKey(ct => ct.Id);

        // Properties
        entity.Property(ct => ct.Provider)
              .HasMaxLength(50)
              .IsRequired();

        entity.Property(ct => ct.Status)
              .HasMaxLength(50)
              .IsRequired();

        entity.Property(ct => ct.ProviderTransactionId)
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(ct => ct.TraceId)
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(ct => ct.ErrorCode)
              .HasMaxLength(20);

        entity.Property(ct => ct.ErrorMessage)
              .HasMaxLength(500);

        entity.Property(ct => ct.RawRequest)
              .HasColumnType("text");

        entity.Property(ct => ct.RawResponse)
              .HasColumnType("text");

        entity.Property(ct => ct.ProviderTimestamp)
              .HasColumnType("timestamp");

        // Relationships
        entity.HasOne(ct => ct.PaymentTransaction)
              .WithOne(pt => pt.ClickTransactions)
              .HasForeignKey<ClickTransaction>(ct => ct.PaymentTransactionId)
              .OnDelete(DeleteBehavior.Cascade);

        // Soft delete filter (if AuditableEntity includes IsDeleted)
        entity.HasQueryFilter(ct => !ct.IsDeleted);

    }
}
