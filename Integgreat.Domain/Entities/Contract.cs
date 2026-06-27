namespace Integgreat.Domain.Entities;

public class Contract
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}