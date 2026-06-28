namespace Integgreat.Application.DTOs.Workspace;

public class WorkspaceMemberResponseDto
{
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
    public string WorkspaceName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
}