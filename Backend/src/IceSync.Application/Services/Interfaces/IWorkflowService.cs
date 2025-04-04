using IceSync.Domain;

namespace IceSync.Application.Services.Interfaces;

public interface IWorkflowService
{
    Task<IList<Workflow>> GetAllByUserId(string userId);

    Task<bool> RunWorkflow(string userId, int workflowId);
}
