namespace IceSync.Api.Responses;

public class WorkflowResponse
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public string WorkflowName { get; set; }
    public bool IsActive { get; set; }
    public string MultiExecBehavior { get; set; }
}
