using IceSync.Application.Interfaces;
using IceSync.Infrastructure.Data.Repositories;
using IceSync.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceSync.Application.Services.Interfaces;
using IceSync.Application.Interfaces.UniversalLoaderApi;
using NSubstitute;
using IceSync.Infrastructure.UniversalLoaderApi.Requests;
using IceSync.Infrastructure.UniversalLoaderApi.Responses;
using IceSync.Domain;
using Microsoft.EntityFrameworkCore;

namespace IceSync.IntegrationTests;

[Collection(nameof(ApiWebApplicationFactory))]
public class SyncWorkflowServiceTests
{
    private readonly AppDbContext _dbContext;
    private readonly ISyncWorkflowsService _syncWorkflowsService;
    private readonly IUniversalLoaderApi _universalLoaderApiMock;
    private readonly IUniversalLoaderAuthApi _universalLoaderAuthApiMock;

    public SyncWorkflowServiceTests(ApiWebApplicationFactory fixture)
    {
        var scope = fixture.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _syncWorkflowsService = scope.ServiceProvider.GetRequiredService<ISyncWorkflowsService>();

        _universalLoaderApiMock = scope.ServiceProvider.GetRequiredService<IUniversalLoaderApi>();
        _universalLoaderAuthApiMock = scope.ServiceProvider.GetRequiredService<IUniversalLoaderAuthApi>();

        // Ensure clean state for each test
        _dbContext.Workflows.RemoveRange(_dbContext.Workflows);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task SyncWorkflows_should_insert_update_and_delete_entries()
    {
        // Arrane
        var workflows = new List<Workflow>
        {
            new Workflow { WorkflowId = 1, WorkflowName = "should be deleted", UserId = "userId1", MultiExecBehavior = "multi" },
            new Workflow { WorkflowId = 2, WorkflowName = "should be deleted", UserId = "userId1", MultiExecBehavior = "multi" },
            new Workflow { WorkflowId = 3, WorkflowName = "should be modified", UserId = "userId1", MultiExecBehavior = "multi" },
        };
        _dbContext.SaveChanges();

        _universalLoaderAuthApiMock.GetAccessTokenAsync(Arg.Any<AuthRequest>()).Returns("token");
        var expectedWorkflows = new List<WorkflowResponse>
            {
                new WorkflowResponse { Id = 3, Name = "modified", MultiExecBehavior = "multi", IsActive = true },
                new WorkflowResponse { Id = 4, Name = "not existing", MultiExecBehavior = "multi", IsActive = true }
            };
        _universalLoaderApiMock.GetWorkflowsAsync().Returns(Task.FromResult<IEnumerable<WorkflowResponse>>(expectedWorkflows));

        // Act
        await _syncWorkflowsService.SyncWorkflows();

        // Assert
        var synced = await _dbContext.Workflows.ToListAsync();
        Assert.Equal(2, synced.Count);
        Assert.True(synced.Any(x => x.WorkflowName == "modified"));
        Assert.True(synced.Any(x => x.WorkflowName == "not existing"));
    }
}
