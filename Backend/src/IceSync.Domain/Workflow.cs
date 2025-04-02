namespace IceSync.Domain;

public class Workflow
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int WorkflowId { get; set; }
    public string WorkflowName { get; set; }
    public bool IsActive { get; set; }
    public string MultiExecBehavior { get; set; }
}