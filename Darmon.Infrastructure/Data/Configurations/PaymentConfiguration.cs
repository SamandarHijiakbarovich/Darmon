using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> entity)
    {
        // Primary key
        entity.HasKey(p => p.Id);

        // Properties
        entity.Property(p => p.Amount)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

        entity.Property(p => p.Currency)
              .HasMaxLength(10)
              .HasDefaultValue("UZS");

        entity.Property(p => p.Description)
              .HasMaxLength(500);

        entity.Property(p => p.ExpirationDate)
              .IsRequired(false);

        // Enum conversions
        entity.Property(p => p.Status)
              .HasConversion(new EnumToStringConverter<PaymentStatus>())
              .HasMaxLength(20);

        // Relationships
        entity.HasOne(p => p.Order)
              .WithOne(o => o.Payment)
              .HasForeignKey<Payment>(p => p.OrderId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(p => p.PaymentTransactions)
              .WithOne(t => t.Payment)
              .HasForeignKey(t => t.PaymentId)
              .OnDelete(DeleteBehavior.Cascade);

        // Soft delete filter
        entity.HasQueryFilter(p => !p.IsDeleted);
    }
}
