namespace Integgreat.Application.DTOs.Admin;

public class AdminWorkspaceHoursDto
{
    public string WorkspaceName { get; set; } = string.Empty;
    public double CompletedHours { get; set; }
    public double EstimatedHours { get; set; }
}