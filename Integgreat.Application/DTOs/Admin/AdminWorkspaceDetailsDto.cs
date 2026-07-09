namespace Integgreat.Application.DTOs.Admin;

public class AdminWorkspaceDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ProjectCount { get; set; }
    public int TotalTaskCount { get; set; }
    public double TotalCompletedHours { get; set; }
    public double TotalEstimatedHours { get; set; }
    public int MemberCount { get; set; }
    public int CompletionPct { get; set; }
    public List<AdminWorkspaceProjectDto> Projects { get; set; } = new();
    public List<AdminWorkspaceMemberDto> Members { get; set; } = new();
}

public class AdminWorkspaceProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CompletionPct { get; set; }
    public double CompletedHours { get; set; }
    public double EstimatedHours { get; set; }
    public int DoneTaskCount { get; set; }
    public int InProgressTaskCount { get; set; }
    public int TodoTaskCount { get; set; }
}

public class AdminWorkspaceMemberDto
{
    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}