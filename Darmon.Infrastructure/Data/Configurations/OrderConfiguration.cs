using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class OrderConfiguration:IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entity)
    {
        // Primary key
        entity.HasKey(o => o.Id);

        // Properties
        entity.Property(o => o.OrderNumber)
              .HasMaxLength(50)
              .IsRequired();

        entity.Property(o => o.TotalAmount)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

        entity.Property(o => o.Status)
              .HasConversion<int>() // Enum to int
              .IsRequired();

        // Relationships
        entity.HasOne(o => o.Courier)
              .WithMany(c => c.Orders)
              .HasForeignKey(o => o.CourierId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(o => o.User)
              .WithMany()
              .HasForeignKey(o => o.UserId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(o => o.Delivery)
              .WithOne(d => d.Order)
              .HasForeignKey<Order>(o => o.DeliveryId)
              .OnDelete(DeleteBehavior.SetNull);

        entity.HasOne(o => o.Payment)
              .WithOne(p => p.Order)
              .HasForeignKey<Order>(o => o.PaymentId)
              .OnDelete(DeleteBehavior.SetNull);

        entity.HasMany(o => o.OrderItems)
              .WithOne(oi => oi.Order)
              .HasForeignKey(oi => oi.OrderId)
              .OnDelete(DeleteBehavior.Cascade);

        // Soft delete filter
        entity.HasQueryFilter(o => !o.IsDeleted);

    }
}
