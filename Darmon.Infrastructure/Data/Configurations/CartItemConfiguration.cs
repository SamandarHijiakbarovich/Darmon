using Darmon.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class CartItemConfiguration: IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> entity)
    {
        entity.HasKey(ci => ci.Id);

        // Properties
        entity.Property(ci => ci.Quantity)
              .IsRequired();

        // Relationships

        // N:1 with Product
        entity.HasOne(ci => ci.Product)
              .WithMany(p => p.CartItems)
              .HasForeignKey(ci => ci.ProductId)
              .OnDelete(DeleteBehavior.Cascade);

        // N:1 with User
        entity.HasOne(ci => ci.User)
              .WithMany(u => u.CartItems)
              .HasForeignKey(ci => ci.UserId)
              .OnDelete(DeleteBehavior.Cascade);

        // Soft delete filter (if AuditableEntity includes IsDeleted)
        entity.HasQueryFilter(ci => !ci.IsDeleted);
    }
}
