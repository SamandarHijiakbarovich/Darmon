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
    public class UserConfiguration:IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> entity)
        {
            // Primary key
            entity.HasKey(u => u.Id);

            // Properties
            entity.Property(u => u.FirstName)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(u => u.LastName)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(u => u.PhoneNumber)
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(u => u.Email)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(u => u.PasswordHash)
                  .IsRequired();

            entity.Property(u => u.Role)
                  .HasConversion<int>() // Enum to int
                  .IsRequired();

            // Relationships
            entity.HasOne(u => u.Address)
                  .WithMany()
                  .HasForeignKey(u => u.AddressId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(u => u.SellerWallet)
                  .WithOne(sw => sw.User)
                  .HasForeignKey<SellerWallet>(sw => sw.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.Orders)
                  .WithOne(o => o.User)
                  .HasForeignKey(o => o.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Notifications)
                  .WithOne(n => n.User)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.ProductReviews)
                  .WithOne(pr => pr.User)
                  .HasForeignKey(pr => pr.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.CartItems)
                  .WithOne(ci => ci.User)
                  .HasForeignKey(ci => ci.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Soft delete filter (if AuditableEntity includes IsDeleted)
            entity.HasQueryFilter(u => !u.IsDeleted);

        }
    }
}
