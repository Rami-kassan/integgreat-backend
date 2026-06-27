using System.Data;

namespace Integgreat.Domain.Entities;

public class WorkspaceMember
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}