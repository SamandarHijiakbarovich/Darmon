using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations
{
    public class DeliveryConfiguration:IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> entity)
        {

            // Primary key
            entity.HasKey(d => d.Id);

            // Properties
            entity.Property(d => d.EstimatedDeliveryTime)
                  .IsRequired();

            entity.Property(d => d.ActualDeliveryTime);

            entity.Property(d => d.Status)
                  .HasConversion<int>() // Enum to int
                  .IsRequired();

            entity.Property(d => d.TrackingNumber)
                  .HasMaxLength(50)
                  .IsRequired();

            // Relationships
            entity.HasOne(d => d.DeliveryAddress)
                  .WithMany()
                  .HasForeignKey(d => d.AddressId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.DeliveryPerson)
                  .WithMany(dp => dp.Deliveries)
                  .HasForeignKey(d => d.DeliveryPersonId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Order)
                  .WithOne(o => o.Delivery)
                  .HasForeignKey<Delivery>(d => d.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Soft delete filter
            entity.HasQueryFilter(d => !d.IsDeleted);
        }
    }
}
