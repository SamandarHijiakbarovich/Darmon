using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class OrderItemConfiguration:IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> entity)
    {
        // Primary key
        entity.HasKey(oi => oi.Id);

        // Properties
        entity.Property(oi => oi.Quantity)
              .IsRequired();

        entity.Property(oi => oi.UnitPrice)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

        // Relationships
        entity.HasOne(oi => oi.Order)
              .WithMany(o => o.OrderItems)
              .HasForeignKey(oi => oi.OrderId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(oi => oi.Product)
              .WithMany(p => p.OrderItems)
              .HasForeignKey(oi => oi.ProductId)
              .OnDelete(DeleteBehavior.Restrict);

        // Soft delete filter (if BaseEntity includes IsDeleted)
        entity.HasQueryFilter(oi => !oi.IsDeleted);
    }
}
