using Integgreat.Application.DTOs.Workspace;

namespace Integgreat.Application.Services;

public interface IWorkspaceService
{
    Task<List<WorkspaceResponseDto>> GetAllAsync();
    Task<bool> IsClientMemberAsync(int clientId, int workspaceId);
    Task<List<WorkspaceResponseDto>> GetAllByClientAsync(int clientId);
    Task<WorkspaceResponseDto?> GetByIdAsync(int id);
    Task<WorkspaceResponseDto> CreateAsync(WorkspaceRequestDto dto);
    Task<WorkspaceResponseDto> UpdateAsync(int id, WorkspaceRequestDto dto);
    Task DeleteAsync(int id);
}