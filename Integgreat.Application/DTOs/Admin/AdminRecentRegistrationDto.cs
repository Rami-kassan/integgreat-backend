namespace Integgreat.Application.DTOs.Admin;

public class AdminRecentRegistrationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Company { get; set; }
}