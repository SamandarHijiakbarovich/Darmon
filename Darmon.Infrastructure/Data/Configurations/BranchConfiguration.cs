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

public class BranchConfiguration: IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> entity)
    {
        // Primary key
        entity.HasKey(b => b.Id);

        // Properties
        entity.Property(b => b.Name)
              .HasMaxLength(150)
              .IsRequired();

        entity.Property(b => b.PhoneNumber)
              .HasMaxLength(20)
              .IsRequired();

        entity.Property(b => b.OpeningTime)
              .IsRequired();

        entity.Property(b => b.ClosingTime)
              .IsRequired();

        // Relationships

        // 1:1 with Address
        entity.HasOne(b => b.Address)
              .WithOne(a => a.Branch)
              .HasForeignKey<Branch>(b => b.AddressId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);

        // 1:N with Products
        entity.HasMany(b => b.Products)
              .WithOne(p => p.Branch)
              .HasForeignKey(p => p.BranchId)
              .OnDelete(DeleteBehavior.Cascade);

        // Soft delete filter (if AuditableEntity includes IsDeleted)
        entity.HasQueryFilter(b => !b.IsDeleted);

    }
}
