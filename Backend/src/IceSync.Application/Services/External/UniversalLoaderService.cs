using Microsoft.Extensions.Options;
using Refit;
using IceSync.Application.Interfaces.UniversalLoaderApi;
using IceSync.Domain;

namespace IceSync.Application.Services.External;

public class UniversalLoaderService : IUniversalLoaderService
{
    private readonly IUniversalLoaderApi _universalLoaderApi;
    private readonly IOptionsMonitor<UniversalLoaderApiOptions> _universalLoaderOptions;

    public UniversalLoaderService(
        IUniversalLoaderApi universalLoaderApi,
        IOptionsMonitor<UniversalLoaderApiOptions> universalLoaderOptions)
    {
        _universalLoaderApi = universalLoaderApi;
        _universalLoaderOptions = universalLoaderOptions;
}

    public async Task<IList<Workflow>> GetWorkflowsAsync()
    {
        var worfklows = await _universalLoaderApi.GetWorkflowsAsync();

        return worfklows.Select(x => new Workflow
        {
            WorkflowId = x.Id,
            WorkflowName = x.Name,
            IsActive = x.IsActive,
            MultiExecBehavior = x.MultiExecBehavior,
            UserId = _universalLoaderOptions.CurrentValue.ApiUserId,
        }).ToList();
    }

    public async Task<bool> RunWorkflowAsync(int workflowId)
    {
        var response = await _universalLoaderApi.RunWorkflowAsync(workflowId);
        return response.IsSuccessStatusCode;
    }
}
