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
    public class ProductConfiguration: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            // Primary key
            entity.HasKey(p => p.Id);

            // Properties
            entity.Property(p => p.Name)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(p => p.Description)
                  .HasMaxLength(1000);

            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(p => p.StockQuantity)
                  .IsRequired();

            entity.Property(p => p.IsPrescriptionRequired)
                  .IsRequired();

            // Relationships
            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Branch)
                  .WithMany(b => b.Products)
                  .HasForeignKey(p => p.BranchId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(p => p.CartItems)
                  .WithOne(ci => ci.Product)
                  .HasForeignKey(ci => ci.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Images)
                  .WithOne(pi => pi.Product)
                  .HasForeignKey(pi => pi.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.OrderItems)
                  .WithOne(oi => oi.Product)
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.Reviews)
                  .WithOne(r => r.Product)
                  .HasForeignKey(r => r.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Soft delete filter (if applicable)
            entity.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
