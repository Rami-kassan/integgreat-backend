using Integgreat.Application.DTOs.Project;

namespace Integgreat.Application.DTOs.Workspace;

public class WorkspaceResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<ProjectResponseDto> Projects { get; set; } = new();
}