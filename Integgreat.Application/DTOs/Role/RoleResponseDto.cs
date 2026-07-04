namespace Integgreat.Application.DTOs.Role;

public class RoleResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? WorkspaceId { get; set; }
    public bool IsGlobal { get; set; }
    public List<string> Permissions { get; set; } = new();
}