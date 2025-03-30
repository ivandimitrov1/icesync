using IceSync.Application.Interfaces;
using IceSync.Application.Interfaces.UniversalLoaderApi;
using IceSync.Application.Services.External;
using IceSync.Application.Services.Interfaces;
using IceSync.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Application.Services;

public class WorkflowService : IWorkflowService
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IUniversalLoaderService _universalLoaderService;

    public WorkflowService(
        IWorkflowRepository workflowRepository,
        IUniversalLoaderService universalLoaderService)
    {
        _workflowRepository = workflowRepository;
        _universalLoaderService = universalLoaderService;
    }

    public Task<IList<Workflow>> GetAllByUserId(string userId)
    {
        return _workflowRepository.GetAllByUserId(userId);
    }

    public Task<bool> RunWorkflow(int workflowId)
    {
        return _universalLoaderService.RunWorkflowAsync(workflowId);
    }
}
