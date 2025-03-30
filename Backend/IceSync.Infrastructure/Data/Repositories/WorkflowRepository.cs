using System;
using Microsoft.EntityFrameworkCore;
using IceSync.Application.Interfaces;
using IceSync.Domain;

namespace IceSync.Infrastructure.Data.Repositories;

public class WorkflowRepository : IWorkflowRepository
{
    private readonly AppDbContext _context;

    public WorkflowRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Workflow>> GetAllByUserId(string userId) =>
        await _context.Workflows.Where(x => x.UserId == userId).ToListAsync();

    public async Task<Workflow?> GetByIdAsync(int id) => 
        await _context.Workflows.FindAsync(id);

    public async Task CreateAsync(Workflow item) => 
        await _context.Workflows.AddAsync(item);

    public async Task CreateAsync(IList<Workflow> items) =>
        await _context.Workflows.AddRangeAsync(items);

    public Task UpdateAsync(Workflow item)
    {
        _context.Entry(item).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(IList<Workflow> items)
    {
        _context.Workflows.RemoveRange(items);
        return Task.CompletedTask;
    }

    public async Task SaveAsync() => 
        await _context.SaveChangesAsync();
}