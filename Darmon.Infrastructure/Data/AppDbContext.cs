using Microsoft.EntityFrameworkCore;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;

namespace Darmon.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseNpgsql("Host=localhost;Port=5432;Database=DarmonDb;Username=Samandar;Password=samandar2004")
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging();
        }
    }

    // DbSets
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<DeliveryPerson> DeliveryPeople { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<SellerWallet> SellerWallets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WithdrawHistory> WithdrawHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Address configuration
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasOne(a => a.Branch)
                .WithOne(b => b.Address)
                .HasForeignKey<Address>(a => a.BranchId)
                .IsRequired(false);

            entity.HasQueryFilter(a => !a.IsDeleted);
        });

        // Branch configuration
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasOne(b => b.Address)
                .WithOne()
                .HasForeignKey<Address>(a => a.BranchId)
                .IsRequired(false);

            entity.HasQueryFilter(b => !b.IsDeleted);
        });

        // CartItem configuration
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(ci => !ci.IsDeleted);
        });

        // Delivery configuration
        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasOne(d => d.DeliveryPerson)
                .WithMany(dp => dp.Deliveries)
                .HasForeignKey(d => d.DeliveryPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(d => d.Status)
                .HasConversion<string>();

            entity.HasQueryFilter(d => !d.IsDeleted);
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(o => o.Delivery)
                .WithOne(d => d.Order)
                .HasForeignKey<Delivery>(d => d.OrderId)
                .IsRequired(false);

            entity.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .IsRequired(false);

            entity.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasIndex(o => o.OrderNumber).IsUnique();
            entity.HasIndex(o => o.Status);

            entity.HasQueryFilter(o => !o.IsDeleted);
        });

        // OrderItem configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(oi => !oi.IsDeleted);
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasOne(p => p.PaymentTransaction)
                .WithOne(t => t.Payment)
                .HasForeignKey<PaymentTransaction>(t => t.PaymentId);

            entity.Property(p => p.Method).HasConversion<string>();
            entity.Property(p => p.Status).HasConversion<string>();

            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(p => p.Name);

            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        // ProductImage configuration
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasQueryFilter(pi => !pi.IsDeleted);
        });

        // ProductReview configuration
        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasQueryFilter(pr => !pr.IsDeleted);
        });

        // SellerWallet configuration
        modelBuilder.Entity<SellerWallet>(entity =>
        {
            entity.HasOne(sw => sw.User)
                .WithOne(u => u.SellerWallet)
                .HasForeignKey<SellerWallet>(sw => sw.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(sw => sw.Balance)
                .HasDefaultValue(0)
                .HasColumnType("decimal(18,2)");

            entity.HasQueryFilter(sw => !sw.IsDeleted);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(u => u.Address)
                .WithMany()
                .HasForeignKey(u => u.AddressId)
                .IsRequired(false);

            // TPH (Table-Per-Hierarchy) konfiguratsiyasi
            entity.HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<DeliveryPerson>("DeliveryPerson");

            // DeliveryPerson uchun alohida filter emas, balki umumiy User filteri
            entity.HasQueryFilter(u => !u.IsDeleted);

            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.PhoneNumber).IsUnique();
            entity.Property(u => u.Role).HasConversion<string>();
        });

        // DeliveryPerson uchun alohida query filter OLIB TASHLANDI
        // Chunki u User jadvalidan meros oladi va faqat root entity (User) uchun filter qo'llash mumkin

        // WithdrawHistory configuration
        modelBuilder.Entity<WithdrawHistory>(entity =>
        {
            entity.HasQueryFilter(wh => !wh.IsDeleted);
        });
    }
}