using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<HoneyShopUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HoneyShopUser>().HasData(
                   new HoneyShopUser
                   {
                       Id = new Guid("11111111-1111-1111-1111-111111111111"),
                       FullName = "Admin User",
                       Email = "admin@honeyshop.com",
                       Password = "Admin123", // Use a hashed password in production
                       Phone = "1234567890",
                       Role = "Admin"
                   }
               );
            // CartProduct Relationships
            modelBuilder.Entity<CartProduct>()
                .HasKey(cp => new { cp.CartItemId }); // Primary Key

            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Cart)
                .WithMany(c => c.CartProducts)
                .HasForeignKey(cp => cp.CartId);

            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CartProducts)
                .HasForeignKey(cp => cp.ProductId);

            // OrderProduct Relationships
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId }); // Composite Key

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);

            // Cart and User Relationship
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId);

            // Order and User Relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
        }
    }
}