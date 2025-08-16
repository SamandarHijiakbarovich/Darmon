using Darmon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Data.Configurations;

public class GatewayTransactionConfiguration: IEntityTypeConfiguration<GatewayTransaction>
{

    public void Configure(EntityTypeBuilder<GatewayTransaction> builder)
    {
        builder.ToTable("GatewayTransactions");

        builder.HasKey(gt => gt.Id);

        builder.Property(gt => gt.Provider)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(gt => gt.Status)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(gt => gt.TraceId)
            .IsRequired()
            .HasMaxLength(100).
            HasDefaultValueSql("NEWID()");

        builder.Property(gt => gt.ErrorCode)
            .HasMaxLength(50);

        builder.HasOne(gt => gt.PaymentTransaction)
    .WithMany(pt => pt.GatewayTransactions)
    .HasForeignKey(gt => gt.PaymentTransactionId)
    .IsRequired()
    .OnDelete(DeleteBehavior.Cascade);
    }
}
