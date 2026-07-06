using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Integgreat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly AppDbContext _context;

    public TimeEntryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TimeEntry>> GetAllByTaskAsync(int taskId)
    {
        return await _context.TimeEntries
            .Where(te => te.TaskId == taskId)
            .OrderByDescending(te => te.LoggedAt)
            .ToListAsync();
    }

    public async Task<TimeEntry?> GetByIdAsync(int id)
    {
        return await _context.TimeEntries
            .FirstOrDefaultAsync(te => te.Id == id);
    }

    public async Task AddAsync(TimeEntry timeEntry)
    {
        await _context.TimeEntries.AddAsync(timeEntry);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var timeEntry = await GetByIdAsync(id);
        if (timeEntry != null)
        {
            _context.TimeEntries.Remove(timeEntry);
            await _context.SaveChangesAsync();
        }
    }
}