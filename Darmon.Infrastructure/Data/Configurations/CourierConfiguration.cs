using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CourierConfiguration : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> entity)
    {
        // Primary key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.FullName)
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(c => c.PhoneNumber)
              .HasMaxLength(20)
              .IsRequired();

        // Relationships
        entity.HasMany(c => c.Orders)
              .WithOne(o => o.Courier)
              .HasForeignKey(o => o.CourierId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
