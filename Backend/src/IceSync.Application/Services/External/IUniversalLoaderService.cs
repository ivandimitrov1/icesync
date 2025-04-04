using Refit;
using IceSync.Domain;

namespace IceSync.Application.Services.External;

public interface IUniversalLoaderService
{
    Task<IList<Workflow>> GetWorkflowsAsync();

    Task<bool> RunWorkflowAsync(int workflowId);
}
