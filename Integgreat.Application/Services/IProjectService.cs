using Integgreat.Application.DTOs.Project;

namespace Integgreat.Application.Services;

public interface IProjectService
{
    Task<List<ProjectResponseDto>> GetAllByWorkspaceAsync(int workspaceId);
    Task<ProjectResponseDto?> GetByIdAsync(int id);
    Task<ProjectResponseDto> CreateAsync(ProjectRequestDto dto);
    Task<ProjectResponseDto> UpdateAsync(int id, ProjectRequestDto dto);
    Task DeleteAsync(int id);
}