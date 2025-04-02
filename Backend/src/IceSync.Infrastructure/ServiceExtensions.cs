using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IceSync.Application.Interfaces;
using IceSync.Infrastructure.Data;
using IceSync.Infrastructure.Data.Repositories;

namespace IceSync.Infrastructure;

public static class ServiceExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("SqlServerConnectionString")));

        services.AddScoped<IWorkflowRepository, WorkflowRepository>();
    }
}