using FoodStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Food" },
            new Category { Id = 2, Name = "Drink" }
        );

        modelBuilder.Entity<Product>()
            .HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(cartItem => cartItem.User)
                .WithMany(user => user.CartItems)
                .HasForeignKey(cartItem => cartItem.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(cartItem => cartItem.Product)
                .WithMany()
                .HasForeignKey(cartItem => cartItem.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(order => order.Status).HasConversion<string>().HasMaxLength(20);
            entity.HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(orderItem => orderItem.Product)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
