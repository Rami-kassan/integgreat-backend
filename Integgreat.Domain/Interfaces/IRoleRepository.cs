using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string name);
    Task<List<Role>> GetAllByWorkspaceAsync(int? workspaceId);
    Task AddAsync(Role role);
}