namespace Integgreat.Application.DTOs.Auth;

public class MeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsSuperAdmin { get; set; }
    public Dictionary<int, List<string>> Permissions { get; set; } = new();
}