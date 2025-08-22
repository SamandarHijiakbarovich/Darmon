using Microsoft.EntityFrameworkCore;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using Darmon.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
    public DbSet<ClickTransaction> ClickTransactions { get; set; }
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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        
    }
}