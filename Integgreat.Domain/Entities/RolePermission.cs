using Integgreat.Domain.Enums;

namespace Integgreat.Domain.Entities;

public class RolePermission
{
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; }
}