using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IceSync.Application.Interfaces;
using IceSync.Application.Interfaces.UniversalLoaderApi;
using IceSync.Application.Services.External;
using IceSync.Application.Services.Interfaces;
using IceSync.Domain;

namespace IceSync.Application.Services.SyncWorkflows;

public class SyncWorkflowsService : ISyncWorkflowsService
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IUniversalLoaderService _universalLoaderService;
    private readonly IOptionsMonitor<UniversalLoaderApiOptions> _universalLoaderApiOptions;
    private readonly ILogger<SyncWorkflowsService> _logger;


    public SyncWorkflowsService(
        IWorkflowRepository workflowRepository,
        IUniversalLoaderService universalLoaderService,
        IOptionsMonitor<UniversalLoaderApiOptions> universalLoaderApiOptions,
        ILogger<SyncWorkflowsService> logger)
    {
        _workflowRepository = workflowRepository;
        _universalLoaderService = universalLoaderService;
        _universalLoaderApiOptions = universalLoaderApiOptions;
        _logger = logger;
    }

    public async Task SyncWorkflows()
    {
        var userId = _universalLoaderApiOptions.CurrentValue.ApiUserId;
        _logger.LogInformation($"syncing started for user id: {userId}");

        try
        {
            await SyncWorkflows(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"syncing for {userId} failed for some reason ..");
        }
    }

    public async Task SyncWorkflows(string userId)
    {
        var syncingWorkflows = await _universalLoaderService.GetWorkflowsAsync();
        _logger.LogInformation($"{syncingWorkflows.Count} workflows to be processed ..");

        var workflowsInDb = (await _workflowRepository.GetAllByUserId(userId))
                            .ToDictionary(x => x.WorkflowId, y => new SyncingWorkflow { Workflow = y, Synced = false });

        var toBeInserted = new List<Workflow>();
        var toBeUpdated = new List<Workflow>();
        var toBeDeleted = new List<Workflow>();

        foreach (var syncingWorkflow in syncingWorkflows)
        {
            if (workflowsInDb.ContainsKey(syncingWorkflow.WorkflowId))
            {
                var workflowInDb = workflowsInDb[syncingWorkflow.WorkflowId].Workflow;
                if (HasWorkflowChanged(workflowInDb, syncingWorkflow))
                {
                    workflowInDb.WorkflowName = syncingWorkflow.WorkflowName;
                    workflowInDb.IsActive = syncingWorkflow.IsActive;
                    workflowInDb.MultiExecBehavior = syncingWorkflow.MultiExecBehavior;

                    toBeUpdated.Add(workflowInDb);
                }

                workflowsInDb[syncingWorkflow.WorkflowId].Synced = true;
            }
            else
            {
                toBeInserted.Add(syncingWorkflow);
            }
        }

        toBeDeleted.AddRange(workflowsInDb.Where(x => x.Value.Synced == false).Select(x => x.Value.Workflow).ToList());

        await _workflowRepository.CreateAsync(toBeInserted);
        await _workflowRepository.DeleteAsync(toBeDeleted);

        _logger.LogInformation($"{toBeInserted.Count} workflows to be inserted ... ");
        _logger.LogInformation($"{toBeDeleted.Count} workflows to be deleted ... ");
        _logger.LogInformation($"{toBeUpdated.Count} workflows to be updated ...");

        if (toBeDeleted.Any() || toBeInserted.Any() || toBeUpdated.Any())
        {
            await _workflowRepository.SaveAsync();
        }
    }

    private bool HasWorkflowChanged(Workflow workflowInDb, Workflow syncingWorkflow)
    {
        var isTheSame =
            workflowInDb.WorkflowName.Equals(syncingWorkflow.WorkflowName)
            && workflowInDb.IsActive.Equals(syncingWorkflow.IsActive)
            && workflowInDb.MultiExecBehavior.Equals(syncingWorkflow.MultiExecBehavior);

        return !isTheSame;
    }
}
