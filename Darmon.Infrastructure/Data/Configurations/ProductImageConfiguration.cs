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
    public class ProductImageConfiguration:IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> entity)
        {
            // Primary key
            entity.HasKey(pi => pi.Id);

            // Properties
            entity.Property(pi => pi.ImageUrl)
                  .HasMaxLength(500)
                  .IsRequired();

            entity.Property(pi => pi.IsMain)
                  .IsRequired();

            // Relationships
            entity.HasOne(pi => pi.Product)
                  .WithMany(p => p.Images)
                  .HasForeignKey(pi => pi.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Soft delete filter (if applicable)
            entity.HasQueryFilter(pi => !pi.IsDeleted);
        }
    }
}
