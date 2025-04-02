using System;
using Microsoft.EntityFrameworkCore;
using IceSync.Domain;
using IceSync.Infrastructure.Data.Configurations;

namespace IceSync.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Workflow> Workflows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WorkflowConfiguration());
    }
}