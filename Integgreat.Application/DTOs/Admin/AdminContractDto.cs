namespace Integgreat.Application.DTOs.Admin;

public class AdminContractDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime UploadedAt { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string WorkspaceName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
}