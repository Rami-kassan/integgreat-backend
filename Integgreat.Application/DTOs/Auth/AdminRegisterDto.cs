namespace Integgreat.Application.DTOs.Auth;

public class AdminRegisterDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsSuperAdmin { get; set; }
    public bool CanManageUsers { get; set; }
}