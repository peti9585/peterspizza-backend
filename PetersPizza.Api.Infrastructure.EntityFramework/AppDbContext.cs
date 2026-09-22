using Microsoft.EntityFrameworkCore;
using PetersPizza.Api.Models.Entities;

namespace PetersPizza.Api.Infrastructure.EntityFramework;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Admin> Admin => Set<Admin>();
    public DbSet<Order> Order => Set<Order>();
    public DbSet<OrderState> OrderState => Set<OrderState>();
    public DbSet<Pizza> Pizza => Set<Pizza>();
    public DbSet<User> User => Set<User>();
    public DbSet<UserRefreshToken> UserRefreshToken => Set<UserRefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pizza>(entity =>
        {
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
        });
    }
}