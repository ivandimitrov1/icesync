using Hangfire;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using IceSync.Api.Filters;
using IceSync.Application;
using IceSync.Infrastructure;
using IceSync.Infrastructure.Data;
using IceSync.Application.Services.SyncWorkflows;
using Hangfire.PostgreSql;

Console.WriteLine("Starting web api ...");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddHealthChecks()
        .AddNpgSql(
        connectionString: connectionString,
        name: "postgresql",
        tags: new[] { "database", "postgresql" });

var allowedHosts = builder.Configuration["CorsAllowedHosts"].Split(";");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins",
        policy =>
        {
            policy.SetIsOriginAllowed(origin => allowedHosts.Contains(new Uri(origin).Host))
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// TO:DO improve it as a separate execution
ApplyMigrations(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.UseAuthorization();

app.MapControllers();

RecurringJob.AddOrUpdate<SyncWorkflowsService>("my-sync-worfklow-job",
    x => x.SyncWorkflows(),
    builder.Configuration["UniversalLoaderApi:SyncJobCronExpression"]);

app.UseCors("AllowedOrigins");

app.MapHealthChecks("/api/health");

app.Run();


void ApplyMigrations(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Check and apply pending migrations
        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            Console.WriteLine("Applying pending migrations...");
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("No pending migrations found.");
        }
    }
}

public partial class Program { }