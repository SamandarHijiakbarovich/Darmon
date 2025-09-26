using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darmon.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
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
              .HasConversion<int>()
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
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull);

        // This is the key change! We remove the explicit one-to-one configuration here.
        // The relationship is now fully defined in PaymentTransactionConfiguration.
        // We only need to define it in one place to avoid conflicts.

        entity.HasMany(o => o.OrderItems)
              .WithOne(oi => oi.Order)
              .HasForeignKey(oi => oi.OrderId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}