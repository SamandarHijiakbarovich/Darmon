using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations
{
    public class WithdrawHistoryConfiguration:IEntityTypeConfiguration<WithdrawHistory>
    {
        public void Configure(EntityTypeBuilder<WithdrawHistory> entity)
        {
            // Primary key
            entity.HasKey(w => w.Id);

            // Properties
            entity.Property(w => w.Amount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(w => w.Status)
                  .HasConversion<int>() // Enum to int
                  .IsRequired();

            entity.Property(w => w.BankAccount)
                  .HasMaxLength(100)
                  .IsRequired();

            // Relationships
            entity.HasOne(w => w.SellerWallet)
                  .WithMany(sw => sw.WithdrawHistories)
                  .HasForeignKey(w => w.SellerWalletId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Soft delete filter (if AuditableEntity includes IsDeleted)
            entity.HasQueryFilter(w => !w.IsDeleted);

        }
    }
}
