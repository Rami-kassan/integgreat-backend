using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IRequestRepository
{
    Task<List<Request>> GetAllByProjectAsync(int projectId);
    Task<Request?> GetByIdAsync(int id);
    Task AddAsync(Request request);
    Task UpdateAsync(Request request);
}