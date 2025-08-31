using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class CategoryConfiguration: IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        // Primary key
        entity.HasKey(c => c.Id);

        // Properties
        entity.Property(c => c.Name)
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(c => c.Description)
              .HasMaxLength(500);

        entity.Property(c => c.ImageUrl)
              .HasMaxLength(300);

        // Relationships
        entity.HasMany(c => c.Products)
              .WithOne(p => p.Category)
              .HasForeignKey(p => p.CategoryId)
              .OnDelete(DeleteBehavior.Restrict);

        // Soft delete filter (if AuditableEntity includes IsDeleted)
        entity.HasQueryFilter(c => !c.IsDeleted);
    }
}
