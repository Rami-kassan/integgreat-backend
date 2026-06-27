namespace Integgreat.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}