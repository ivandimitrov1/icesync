using IceSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using Testcontainers.MsSql;

namespace IceSync.IntegrationTests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;
    public AppDbContext DbContext { get; private set; }

    public DatabaseFixture()
    {
        _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Password123") // SQL Server requires strong password
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        // Configure DbContext
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_dbContainer.GetConnectionString())
            .Options;

        DbContext = new AppDbContext(options);


        await DbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}
