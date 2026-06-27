namespace Integgreat.Domain.Entities;

public class Admin : User
{
    public bool IsSuperAdmin { get; set; }
    public bool CanManageUsers { get; set; }
    public DateTime? LastLogin { get; set; }
}