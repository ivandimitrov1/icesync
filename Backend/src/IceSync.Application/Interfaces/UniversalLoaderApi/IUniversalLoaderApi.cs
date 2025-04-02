using Refit;
using IceSync.Infrastructure.UniversalLoaderApi.Responses;

namespace IceSync.Application.Interfaces.UniversalLoaderApi;

public interface IUniversalLoaderApi
{
    [Get("/workflows")]
    Task<IEnumerable<WorkflowResponse>> GetWorkflowsAsync();

    [Post("/workflows/{workflowId}/run")]
    Task<IApiResponse> RunWorkflowAsync(int workflowId);
}
