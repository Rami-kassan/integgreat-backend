using Integgreat.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Integgreat.Domain.Interfaces;

public interface IRequestRepository
{
    Task<List<Request>> GetAllByProjectAsync(int projectId);
    Task<Request?> GetByIdAsync(int id);
    Task AddAsync(Request request);
    Task UpdateAsync(Request request);
    Task<List<Request>> GetAllAsync();
    Task<List<Request>> GetRecentAsync(int count);
}