using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Integgreat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectTask>> GetAllByProjectAsync(int projectId)
    {
        return await _context.Tasks
            .Include(t => t.TimeEntries)
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<ProjectTask?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.TimeEntries) 
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(ProjectTask task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProjectTask task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var task = await GetByIdAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}