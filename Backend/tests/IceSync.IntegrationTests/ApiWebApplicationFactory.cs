using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using IceSync.Application.Interfaces.UniversalLoaderApi;
using NSubstitute;
using Testcontainers.PostgreSql;

namespace IceSync.IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:DefaultConnectionString", _dbContainer.GetConnectionString());

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
