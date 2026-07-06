using Integgreat.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Integgreat.Domain.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllByWorkspaceAsync(int workspaceId);
    Task<Project?> GetByIdAsync(int id);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(int id);
    Task<List<Project>> GetAllAsync();
    Task<List<Project>> GetAllWithDetailsAsync();
}