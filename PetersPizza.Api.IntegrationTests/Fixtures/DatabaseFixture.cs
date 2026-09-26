using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetersPizza.Api.Infrastructure.EntityFramework;
using Testcontainers.PostgreSql;

namespace PetersPizza.Api.IntegrationTests.Fixtures;

public sealed class DatabaseFixture : IAsyncDisposable
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder("postgres:18")
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        await using var db = CreateDbContext();
        
        await db.Database.MigrateAsync();
    }
    
    public AppDbContext CreateDbContext()
    {
        var connectionString = _postgres.GetConnectionString();
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(
                connectionString,
                sql => sql.MigrationsAssembly("PetersPizza.Database"))
            .Options;
        
        return new AppDbContext(options);
    }
    
    public async ValueTask DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }
}