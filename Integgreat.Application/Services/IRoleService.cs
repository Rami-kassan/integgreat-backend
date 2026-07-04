using Integgreat.Application.DTOs.Role;

namespace Integgreat.Application.Services;

public interface IRoleService
{
    Task<List<RoleResponseDto>> GetAllGlobalAsync();
    Task<List<RoleResponseDto>> GetAllByWorkspaceIncludingGlobalAsync(int workspaceId);
    Task<RoleResponseDto> CreateAsync(RoleRequestDto dto);
    Task UpdatePermissionsAsync(int roleId, List<string> permissions);
}