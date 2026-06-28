using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllByWorkspaceAsync(int workspaceId);
    Task<Project?> GetByIdAsync(int id);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(int id);
}