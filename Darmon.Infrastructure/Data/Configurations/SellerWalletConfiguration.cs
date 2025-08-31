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
    public class SellerWalletConfiguration:IEntityTypeConfiguration<SellerWallet>
    {
        public void Configure(EntityTypeBuilder<SellerWallet> entity)
        {
            // Primary key
            entity.HasKey(sw => sw.Id);

            // Properties
            entity.Property(sw => sw.Balance)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            // Relationships
            entity.HasOne(sw => sw.User)
                  .WithOne(u => u.SellerWallet)
                  .HasForeignKey<SellerWallet>(sw => sw.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(sw => sw.WithdrawHistories)
                  .WithOne(wh => wh.SellerWallet)
                  .HasForeignKey(wh => wh.SellerWalletId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Soft delete filter (if BaseEntity includes IsDeleted)
            entity.HasQueryFilter(sw => !sw.IsDeleted);
        }
    }
}
