namespace Integgreat.Application.DTOs.Admin;

public class AdminUserDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsSuperAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Company { get; set; }
    public string? Phone { get; set; }
    public List<AdminUserWorkspaceDto> Workspaces { get; set; } = new();
    public List<AdminUserProjectDto> Projects { get; set; } = new();
}

public class AdminUserWorkspaceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProjectCount { get; set; }
    public int MemberCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AdminUserProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double EstimatedHours { get; set; }
    public double CompletedHours { get; set; }
    public int TaskCount { get; set; }
    public int DoneTaskCount { get; set; }
    public int Pct { get; set; }
}