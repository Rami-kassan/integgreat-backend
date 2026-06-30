using Integgreat.Application.DTOs.Workspace;

namespace Integgreat.Application.Services;

public interface IWorkspaceMemberService
{
    Task<List<WorkspaceMemberResponseDto>> GetAllByWorkspaceAsync(int workspaceId);
    Task<WorkspaceMemberResponseDto> AddMemberAsync(WorkspaceMemberRequestDto dto);
    Task RemoveMemberAsync(int clientId, int workspaceId);
}