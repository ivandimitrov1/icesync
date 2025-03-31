using IceSync.Application.Interfaces;
using IceSync.Domain;
using IceSync.Infrastructure.Data;
using IceSync.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IceSync.IntegrationTests.Repositories;

[Collection(nameof(ApiWebApplicationFactory))]
public class WorkflowRepositoryTests
{
    private readonly AppDbContext _dbContext;
    private readonly IWorkflowRepository _workflowRepository;

    public WorkflowRepositoryTests(ApiWebApplicationFactory fixture)
    {
        var scope = fixture.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _workflowRepository = new WorkflowRepository(_dbContext);

        // Ensure clean state for each test
        _dbContext.Workflows.RemoveRange(_dbContext.Workflows);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllByUserId_ShouldReturnOnlyUsersWorkflows()
    {
        // Arrange
        var userId1 = "user1";
        var userId2 = "user2";

        var workflows = new List<Workflow>
        {
            new Workflow { WorkflowName = "Workflow 1", UserId = userId1, MultiExecBehavior = "multi" },
            new Workflow { WorkflowName = "Workflow 2", UserId = userId1, MultiExecBehavior = "multi"},
            new Workflow { WorkflowName = "Workflow 2", UserId = userId2, MultiExecBehavior = "multi"},
        };

        await _workflowRepository.CreateAsync(workflows);
        await _workflowRepository.SaveAsync();

        // Act
        var result = await _workflowRepository.GetAllByUserId(userId1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, w => Assert.Equal(userId1, w.UserId));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveWorkflows()
    {
        // Arrange
        var workflows = new List<Workflow>
        {
            new Workflow { WorkflowName = "To Delete 1", UserId = "user1", MultiExecBehavior = "multi" },
            new Workflow { WorkflowName = "To Delete 2", UserId = "user1", MultiExecBehavior = "multi"  },
            new Workflow { WorkflowName = "Keep Me", UserId = "user2", MultiExecBehavior = "multi"  }
        };

        await _workflowRepository.CreateAsync(workflows);
        await _workflowRepository.SaveAsync();

        var workflowsToDelete = workflows.Take(2).ToList();

        // Act
        await _workflowRepository.DeleteAsync(workflowsToDelete);
        await _workflowRepository.SaveAsync();

        // Assert
        var remainingWorkflows = _dbContext.Workflows.ToList();
        Assert.Single(remainingWorkflows);
        Assert.Equal("Keep Me", remainingWorkflows[0].WorkflowName);
    }

    [Fact]
    public async Task SaveAsync_ShouldPersistChanges()
    {
        // Arrange
        var workflow = new Workflow { WorkflowName = "Unsaved Workflow", UserId = "user1", MultiExecBehavior = "multi" };
        await _workflowRepository.CreateAsync(new List<Workflow> { workflow });

        // Pre-assert (not saved yet)
        var beforeSave = _dbContext.Workflows.FirstOrDefault();
        Assert.Null(beforeSave);

        // Act
        await _workflowRepository.SaveAsync();

        // Assert
        var afterSave = _dbContext.Workflows.FirstOrDefault();
        Assert.NotNull(afterSave);
        Assert.Equal("Unsaved Workflow", afterSave.WorkflowName);
    }
}