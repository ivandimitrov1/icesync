using Microsoft.EntityFrameworkCore;
using IceSync.Application.Interfaces;
using IceSync.Domain;

namespace IceSync.Infrastructure.Data.Repositories;

// TO:DO 
// add a base repo
public class WorkflowRepository : IWorkflowRepository
{
    private readonly AppDbContext _context;

    public WorkflowRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Workflow>> GetAllByUserId(string userId) =>
        await _context.Workflows.Where(x => x.UserId == userId).ToListAsync();

    public async Task CreateAsync(IList<Workflow> items) =>
        await _context.Workflows.AddRangeAsync(items);

    public Task DeleteAsync(IList<Workflow> items)
    {
        _context.Workflows.RemoveRange(items);
        return Task.CompletedTask;
    }

    public async Task SaveAsync() => 
        await _context.SaveChangesAsync();

    public async Task<Workflow?> GetByWorfklowId(string userId, int workflowId)
    {
        return await _context
                    .Workflows
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.WorkflowId == workflowId);
    }
}