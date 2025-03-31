using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using IceSync.Application.Interfaces.UniversalLoaderApi;
using Refit;
using IceSync.Application.Services.External;
using IceSync.Application.Services.Interfaces;
using IceSync.Application.Services.SyncWorkflows;

namespace IceSync.Application;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // TO:DO setup with validation
        services.Configure<UniversalLoaderApiOptions>(configuration.GetSection(UniversalLoaderApiOptions.Section));

        services.AddRefitClient<IUniversalLoaderAuthApi>()
            .ConfigureHttpClient(c => 
            {
                c.BaseAddress = new Uri(configuration["UniversalLoaderApi:ApiUrl"]);
                });

        services.AddRefitClient<IUniversalLoaderApi>()
            .ConfigureHttpClient(c => {
                c.BaseAddress = new Uri(configuration["UniversalLoaderApi:ApiUrl"]);
                })
            .AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddScoped<AuthHeaderHandler>();
        services.AddScoped<IUniversalLoaderService, UniversalLoaderService>();

        services.AddScoped<IWorkflowService, WorkflowService>();
        services.AddScoped<ISyncWorkflowsService, SyncWorkflowsService>();
    }
}