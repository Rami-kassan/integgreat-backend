namespace Integgreat.Application.DTOs.Admin;

public class AdminUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "CLIENT" ou "ADMIN"
    public bool IsSuperAdmin { get; set; }
    public DateTime CreatedAt { get; set; }

    // Si Client
    public string? Company { get; set; }
    public string? Phone { get; set; }
}