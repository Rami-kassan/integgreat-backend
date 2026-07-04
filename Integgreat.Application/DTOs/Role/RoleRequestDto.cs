namespace Integgreat.Application.DTOs.Role;

public class RoleRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
    public List<string> Permissions { get; set; } = new();
}