// AdminWorkspaceDto.cs
public class AdminWorkspaceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ProjectCount { get; set; }
    public int TotalTaskCount { get; set; }
    public int MemberCount { get; set; }
    public int CompletionPct { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
}