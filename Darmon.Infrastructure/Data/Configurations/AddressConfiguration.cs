using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations
{
    public class AddressConfiguration:IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            // Primary key
            entity.HasKey(a => a.Id);

            // Properties
            entity.Property(a => a.Street)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(a => a.City)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(a => a.PostalCode)
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(a => a.Landmark)
                  .HasMaxLength(200);

            entity.Property(a => a.Latitude)
                  .HasColumnType("double precision");

            entity.Property(a => a.Longitude)
                  .HasColumnType("double precision");

            // Relationships
            entity.HasOne(a => a.User)
                  .WithMany()
                  .HasForeignKey(a => a.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.Branch)
                  .WithOne(b => b.Address)
                  .HasForeignKey<Address>(a => a.BranchId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);

            // Soft delete filter (if BaseEntity includes IsDeleted)
            entity.HasQueryFilter(a => !a.IsDeleted);
        }
    }
}
