using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface ITaskRepository
{
    Task<List<ProjectTask>> GetAllByProjectAsync(int projectId);
    Task<ProjectTask?> GetByIdAsync(int id);
    Task AddAsync(ProjectTask task);
    Task UpdateAsync(ProjectTask task);
    Task DeleteAsync(int id);
    Task<List<ProjectTask>> GetAllAsync();
}