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
            
        modelBuilder.Entity<OrderState>()
            .HasData(Enum.GetValues<Models.Common.OrderState>()
            .Where(e => e != Models.Common.OrderState.Undefined)
            .Select(os => new OrderState
            {
                Id = (int)os,
                Name = os.ToString(),
                CreatedAt = new DateTime(2026, 9, 24, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "System"
            }));
    }
}