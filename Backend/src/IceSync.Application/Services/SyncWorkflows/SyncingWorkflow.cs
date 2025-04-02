using IceSync.Domain;

namespace IceSync.Application.Services.SyncWorkflows;

public class SyncingWorkflow
{
    public Workflow Workflow { get; set; }

    public bool Synced { get; set; }
}
