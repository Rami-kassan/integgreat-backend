using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IWorkspaceRepository
{
    Task<List<Workspace>> GetAllAsync();

    Task<List<Workspace>> GetAllByClientAsync(int clientId);
    Task<Workspace?> GetByIdAsync(int id);
    Task AddAsync(Workspace workspace);
    Task UpdateAsync(Workspace workspace);
    Task DeleteAsync(int id);
    Task<List<Workspace>> GetAllWithDetailsAsync();
    Task<Workspace?> GetByIdWithDetailsAsync(int id);
}
