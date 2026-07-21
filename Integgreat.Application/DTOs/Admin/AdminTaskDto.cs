namespace Integgreat.Application.DTOs.Admin;

public class AdminTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double EstimatedHours { get; set; }
    public double CompletedHours { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
    public string WorkspaceName { get; set; } = string.Empty;
}
