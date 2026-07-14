using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IContractRepository
{
    Task<List<Contract>> GetAllByProjectAsync(int projectId);
    Task<Contract?> GetActiveByProjectAsync(int projectId);
    Task AddAsync(Contract contract);
    Task UpdateAsync(Contract contract);
    Task<List<Contract>> GetAllWithDetailsAsync();
    Task<Contract?> GetByIdAsync(int id);
}