using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations
{
    public class ProductReviewConfiguration:IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> entity)
        {

            // Primary key
            entity.HasKey(pr => pr.Id);

            // Properties
            entity.Property(pr => pr.Comment)
                  .HasMaxLength(1000)
                  .IsRequired();

            entity.Property(pr => pr.Rating)
                  .IsRequired();

            // Relationships
            entity.HasOne(pr => pr.Product)
                  .WithMany(p => p.Reviews)
                  .HasForeignKey(pr => pr.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pr => pr.User)
                  .WithMany(u => u.ProductReviews)
                  .HasForeignKey(pr => pr.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Soft delete filter (if BaseEntity includes IsDeleted)
            entity.HasQueryFilter(pr => !pr.IsDeleted);
        }
    }
}
