using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data.Models;

namespace OnlineBookStoreAPI.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderDetail> OrderDetails { get; set; }
    
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>().HasKey(e => e.Id);
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");

        // Order relationship
        modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders);

        // OrderDetail relationship
        modelBuilder.Entity<OrderDetail>().HasOne(od => od.Order).WithMany(o => o.OrderDetails);
        modelBuilder.Entity<OrderDetail>().HasOne(od => od.Book).WithMany(o => o.OrderDetails);

        // Review relationship
        modelBuilder.Entity<Review>().HasOne(r => r.Book).WithMany(b => b.Reviews);
        modelBuilder.Entity<Review>().HasOne(r => r.User).WithMany(u => u.Reviews);
    }
}