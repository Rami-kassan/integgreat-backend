namespace Integgreat.Application.DTOs.Admin;

public class AdminProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
    public string WorkspaceName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int CompletionPct { get; set; }
    public double CompletedHours { get; set; }
    public double EstimatedHours { get; set; }
    public int TaskCount { get; set; }
    public int DoneTaskCount { get; set; }
}