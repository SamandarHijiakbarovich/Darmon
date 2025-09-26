using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Darmon.Infrastructure.Data.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> entity)
    {
        // Primary key - Asosiy kalit
        entity.HasKey(pt => pt.Id);

        // Xususiyatlar (Properties)
        entity.Property(pt => pt.Amount)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

        entity.Property(pt => pt.TransactionId)
              .IsRequired()
              .HasMaxLength(100);

        entity.Property(pt => pt.PaymentTransId)
              .HasMaxLength(200);

        entity.Property(pt => pt.CreatedAt)
              .IsRequired();

        entity.Property(pt => pt.Status)
              .HasConversion<int>()
              .IsRequired();

        entity.Property(pt => pt.PaymentType)
              .HasConversion<int>()
              .IsRequired();

        // Aloqalar (Relationships)

        // Foydalanuvchi bilan aloqa
        entity.HasOne(pt => pt.User)
              .WithMany()
              .HasForeignKey(pt => pt.UserId)
              .OnDelete(DeleteBehavior.NoAction);

        // This is the key change! This single line defines the one-to-one relationship.
        // It says: "A PaymentTransaction has one Order, and that Order has one PaymentTransaction."
        entity.HasOne(pt => pt.Order)
              .WithOne(o => o.PaymentTransaction)
              .HasForeignKey<PaymentTransaction>(pt => pt.OrderId)
              .IsRequired(false) // An order is not mandatory at the time of transaction creation
              .OnDelete(DeleteBehavior.SetNull); // If the order is deleted, set the OrderId to null.
    }
}