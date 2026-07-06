using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface ITimeEntryRepository
{
    Task<List<TimeEntry>> GetAllByTaskAsync(int taskId);
    Task<TimeEntry?> GetByIdAsync(int id);
    Task AddAsync(TimeEntry timeEntry);
    Task DeleteAsync(int id);
}