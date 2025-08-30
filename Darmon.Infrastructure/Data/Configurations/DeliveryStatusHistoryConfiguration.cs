using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations
{
    public class DeliveryStatusHistoryConfiguration: IEntityTypeConfiguration<DeliveryStatusHistory>
    {
        public void Configure(EntityTypeBuilder<DeliveryStatusHistory> entity)
        {
            // Primary key
            entity.HasKey(dsh => dsh.Id);

            // Properties
            entity.Property(dsh => dsh.Status)
                  .HasConversion<int>() // Enum to int
                  .IsRequired();

            entity.Property(dsh => dsh.ChangedAt)
                  .IsRequired();

            entity.Property(dsh => dsh.Notes)
                  .HasMaxLength(500);

            // Relationships
            entity.HasOne(dsh => dsh.Delivery)
                  .WithMany(d => d.StatusHistories)
                  .HasForeignKey(dsh => dsh.DeliveryId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(dsh => dsh.ChangedBy)
                  .WithMany()
                  .HasForeignKey(dsh => dsh.ChangedById)
                  .OnDelete(DeleteBehavior.Restrict);

            // Optional: Soft delete filter (if needed)
            // entity.HasQueryFilter(dsh => !dsh.IsDeleted);
        }
    }
}
