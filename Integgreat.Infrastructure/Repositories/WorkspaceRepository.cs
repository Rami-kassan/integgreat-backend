using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Integgreat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly AppDbContext _context;

    public WorkspaceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Workspace>> GetAllAsync()
    {
        return await _context.Workspaces
            .Include(w => w.Members)
            .Include(w => w.Projects)
                .ThenInclude(p => p.Tasks)
                    .ThenInclude(t => t.TimeEntries)
            .ToListAsync();
    }

    public async Task<List<Workspace>> GetAllByClientAsync(int clientId)
    {
        return await _context.WorkspaceMembers
            .Where(wm => wm.ClientId == clientId)
            .Include(wm => wm.Workspace)
                .ThenInclude(w => w.Projects)
                    .ThenInclude(p => p.Tasks)
                        .ThenInclude(t => t.TimeEntries)
            .Select(wm => wm.Workspace)
            .ToListAsync();
    }

    public async Task<Workspace?> GetByIdAsync(int id)
    {
        return await _context.Workspaces
            .Include(w => w.Members)
            .Include(w => w.Roles)
            .Include(w => w.Projects)
                .ThenInclude(p => p.Tasks)
                    .ThenInclude(t => t.TimeEntries)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task AddAsync(Workspace workspace)
    {
        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Workspace workspace)
    {
        _context.Workspaces.Update(workspace);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var workspace = await GetByIdAsync(id);
        if (workspace != null)
        {
            _context.Workspaces.Remove(workspace);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<List<Workspace>> GetAllWithDetailsAsync()
    {
        return await _context.Workspaces
            .Include(w => w.Projects)
                .ThenInclude(p => p.Tasks)
                    .ThenInclude(t => t.TimeEntries)
            .Include(w => w.Members)
                .ThenInclude(m => m.Client)
            .Include(w => w.Members)
                .ThenInclude(m => m.Role)
            .ToListAsync();
    }

    public async Task<Workspace?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Workspaces
            .Include(w => w.Projects)
                .ThenInclude(p => p.Tasks)
                    .ThenInclude(t => t.TimeEntries)
            .Include(w => w.Members)
                .ThenInclude(m => m.Client)
            .Include(w => w.Members)
                .ThenInclude(m => m.Role)
            .FirstOrDefaultAsync(w => w.Id == id);
    }
}