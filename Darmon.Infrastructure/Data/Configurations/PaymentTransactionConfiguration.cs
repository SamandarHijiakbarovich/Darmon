using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class PaymentTransactionConfiguration:IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> entity)
    {
        // Primary key
        entity.HasKey(pt => pt.Id);

        // Properties
        entity.Property(pt => pt.Amount)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

        entity.Property(pt => pt.InternalTraceId)
              .IsRequired();

        entity.Property(pt => pt.Status)
              .HasConversion<int>() // Enum to int
              .IsRequired();

        entity.Property(pt => pt.ClientRedirectUrl)
              .HasMaxLength(500);

        entity.Property(pt => pt.CallbackUrl)
              .HasMaxLength(500);

        entity.Property(pt => pt.ErrorMessage)
              .HasMaxLength(1000);

        entity.Property(pt => pt.GatewaySessionId)
              .HasMaxLength(200);

        // Relationships
        entity.HasOne(pt => pt.Payment)
              .WithMany(p => p.PaymentTransactions)
              .HasForeignKey(pt => pt.PaymentId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(pt => pt.ClickTransactions)
              .WithOne(ct => ct.PaymentTransaction)
              .HasForeignKey<ClickTransaction>(ct => ct.PaymentTransactionId)
              .OnDelete(DeleteBehavior.Cascade);

        // Soft delete filter (if applicable)
        entity.HasQueryFilter(pt => !pt.IsDeleted);
    }
}
