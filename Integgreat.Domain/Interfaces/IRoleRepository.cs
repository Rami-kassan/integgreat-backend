using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string name);
    Task<List<Role>> GetAllByWorkspaceAsync(int? workspaceId);
    Task AddAsync(Role role);
    Task<List<Role>> GetAllGlobalAsync();
    Task<List<Role>> GetAllByWorkspaceIncludingGlobalAsync(int workspaceId);
    Task<Role?> GetByIdWithPermissionsAsync(int id);
    Task UpdateAsync(Role role);
    Task<int?> GetWorkspaceIdByRoleIdAsync(int roleId);
}