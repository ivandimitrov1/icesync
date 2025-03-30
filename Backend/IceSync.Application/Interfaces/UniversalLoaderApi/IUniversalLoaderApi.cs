using Refit;
using IceSync.Infrastructure.UniversalLoaderApi.Requests;
using IceSync.Infrastructure.UniversalLoaderApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Interfaces.UniversalLoaderApi;

public interface IUniversalLoaderApi
{
    [Get("/workflows")]
    Task<IEnumerable<WorkflowResponse>> GetWorkflowsAsync();

    [Post("/workflows/{workflowId}/run")]
    Task<IApiResponse> RunWorkflowAsync(int workflowId);
}
