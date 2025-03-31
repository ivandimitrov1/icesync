using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore;
using IceSync.Infrastructure.Data;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IceSync.Application.Interfaces.UniversalLoaderApi;
using NSubstitute;

namespace IceSync.IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Password123")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:SqlServerConnectionString", _dbContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<IUniversalLoaderApi>(Substitute.For<IUniversalLoaderApi>());
            services.AddSingleton<IUniversalLoaderAuthApi>(Substitute.For<IUniversalLoaderAuthApi>());
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
