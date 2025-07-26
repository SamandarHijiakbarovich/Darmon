using Microsoft.EntityFrameworkCore;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;

namespace Darmon.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql("Host=localhost;Port=5433;Database=DarmonDb;Username=Samandar;Password=samandar2004");
            }
        }
        // Asosiy DbSetlar
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SellerWallet> SellerWallets { get; set; }
        public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User konfiguratsiyalari
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId);

                entity.HasMany(u => u.Notifications)
                    .WithOne(n => n.User)
                    .HasForeignKey(n => n.UserId);

                entity.OwnsOne(u => u.Address); // Address owned type sifatida
            });

            // Product konfiguratsiyalari
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasMany(p => p.Images)
                    .WithOne()
                    .HasForeignKey(pi => pi.ProductId);

                entity.HasMany(p => p.Reviews)
                    .WithOne(r => r.Product)
                    .HasForeignKey(r => r.ProductId);

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId);
            });

            // Order konfiguratsiyalari
            modelBuilder.Entity<Order>(entity =>
            {
                // Order -> OrderItems (One-to-Many)
                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)  // OrderItem ichidagi Order navigation property nomi
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);  // Order o'chganda OrderItems ham o'chadi

                // Order -> Delivery (One-to-One)
                entity.HasOne(o => o.Delivery)
                    .WithOne(d => d.Order)
                    .HasForeignKey<Delivery>(d => d.OrderId)  // Delivery jadvalidagi OrderId foreign key
                    .IsRequired(false);  // Optional relationship

                // Order -> Payment (One-to-One)
                entity.HasOne(o => o.Payment)
                    .WithOne(p => p.Order)
                    .HasForeignKey<Payment>(p => p.OrderId)
                    .IsRequired(false);  // Optional relationship

                // Order -> User (Many-to-One)
                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);  // User o'chganda Orderlar o'chmasin

                // Enum conversion
                entity.Property(o => o.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);  // DBda string sifatida saqlash uchun max length

                // Indexes
                entity.HasIndex(o => o.OrderNumber)
                    .IsUnique();

                entity.HasIndex(o => o.Status);
            });


            modelBuilder.Entity<ProductReview>(entity =>
            {
                // ProductReview -> Product (Many-to-One)
                entity.HasOne(pr => pr.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(pr => pr.ProductId)
                    .OnDelete(DeleteBehavior.Cascade); // Product o'chganda reviewlar ham o'chadi

                // ProductReview -> User (Many-to-One)
                entity.HasOne(pr => pr.User)
                    .WithMany()
                    .HasForeignKey(pr => pr.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // User o'chganda reviewlar o'chmasin
            });

            // Payment konfiguratsiyalari
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasOne(p => p.PaymentTransaction)
                    .WithOne(t => t.Payment)
                    .HasForeignKey<PaymentTransaction>(t => t.PaymentId);

                entity.Property(p => p.Method)
                    .HasConversion<string>();

                entity.Property(p => p.Status)
                    .HasConversion<string>();
            });

            // Delivery konfiguratsiyalari
            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasOne(d => d.DeliveryPerson)
                    .WithMany()
                    .HasForeignKey(d => d.DeliveryPersonId);

                entity.Property(d => d.Status)
                    .HasConversion<string>();
            });

            // order konfiguratsiyalari
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // One-to-One konfiguratsiya
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasOne(b => b.Address)
                      .WithOne()
                      .HasForeignKey<Address>(a => a.BranchId)
                      .IsRequired(false); // Agar optional bo'lsa
            });


            // Enum konfiguratsiyalari
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // Global query filter (soft delete uchun)
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);

            // Indexlar
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}