using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using IceSync.Api.Responses;
using IceSync.Application.Interfaces.UniversalLoaderApi;
using IceSync.Application.Services.Interfaces;
using IceSync.Domain;

namespace IceSync.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkflowsController : ControllerBase
{
    private readonly IWorkflowService _workflowService;
    private readonly ISyncWorkflowsService _syncWorkflowService;
    private readonly ILogger<WorkflowsController> _logger;
    private readonly IOptionsMonitor<UniversalLoaderApiOptions> _universalLoaderApiOptions;

    public WorkflowsController(
        IWorkflowService workflowService,
        ISyncWorkflowsService syncWorkflowsService,
        ILogger<WorkflowsController> logger,
        IOptionsMonitor<UniversalLoaderApiOptions> universalLoaderApiOptions)
    {
        _logger = logger;
        _workflowService = workflowService;
        _universalLoaderApiOptions = universalLoaderApiOptions;
        _syncWorkflowService = syncWorkflowsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkflowResponse>>> GetWorkflows()
    {
        var userId = _universalLoaderApiOptions.CurrentValue.ApiUserId;

        try
        {
            var workflows = await _workflowService.GetAllByUserId(userId);
            return Ok(MapToRest(workflows));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting all workflows for {userId}.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("{workflowId}/run")]
    public async Task<ActionResult> RunWorkflow(int workflowId)
    {
        var userId = _universalLoaderApiOptions.CurrentValue.ApiUserId;

        try
        {
            if (await _workflowService.RunWorkflow(userId, workflowId))
            {
                return Ok();
            }

            return Conflict();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error running a worfklow for {userId}");
            return StatusCode(500, "Internal server error");
        }
    }

    private IList<WorkflowResponse> MapToRest(IList<Workflow> workflows)
    {
        return workflows.Select(x => new WorkflowResponse
        {
            Id = x.Id,
            WorkflowId = x.WorkflowId,
            WorkflowName = x.WorkflowName,
            IsActive = x.IsActive,
            MultiExecBehavior = x.MultiExecBehavior
        }).ToList();
    }
}