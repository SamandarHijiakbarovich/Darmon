using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class DeliveryPersonConfiguration : IEntityTypeConfiguration<DeliveryPerson>
{
    public void Configure(EntityTypeBuilder<DeliveryPerson> entity)
    {
        // Primary key inherited from User
        entity.HasKey(dp => dp.Id);

        // Properties
        entity.Property(dp => dp.VehicleNumber)
              .HasMaxLength(20)
              .IsRequired();

        entity.Property(dp => dp.IsAvailable)
              .IsRequired();

        entity.Property(dp => dp.VehicleType)
              .HasConversion<int>() // Enum to int
              .IsRequired();

        // Relationships
        entity.HasMany(dp => dp.Deliveries)
              .WithOne(d => d.DeliveryPerson)
              .HasForeignKey(d => d.DeliveryPersonId)
              .OnDelete(DeleteBehavior.SetNull);
    }
}
