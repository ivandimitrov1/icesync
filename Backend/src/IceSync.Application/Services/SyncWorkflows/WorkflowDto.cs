using IceSync.Domain;

namespace IceSync.Application.Services.SyncWorkflows;

public class WorkflowDto
{
    public Workflow Workflow { get; set; }

    public bool Synced { get; set; }
}
