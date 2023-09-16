using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data.Models;

namespace OnlineBookStoreAPI.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderDetail> OrderDetails { get; set; }
    
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>().HasKey(e => e.Id);
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");

        modelBuilder.Entity<Book>().ToTable("Books");
        modelBuilder.Entity<Book>().HasKey(e => e.Id);

        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders);

        modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
        modelBuilder.Entity<OrderDetail>().HasOne(od => od.Order).WithMany(o => o.OrderDetails);
        modelBuilder.Entity<OrderDetail>().HasOne(od => od.Book).WithMany(o => o.OrderDetails);

        modelBuilder.Entity<Review>().ToTable("Reviews");
        modelBuilder.Entity<Review>().HasOne(r => r.Book).WithMany(b => b.Reviews);
        modelBuilder.Entity<Review>().HasOne(r => r.User).WithMany(u => u.Reviews);
    }
}