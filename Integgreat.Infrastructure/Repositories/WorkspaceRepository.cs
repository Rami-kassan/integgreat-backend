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
            .ToListAsync();
    }

    public async Task<Workspace?> GetByIdAsync(int id)
    {
        return await _context.Workspaces
            .Include(w => w.Members)
            .Include(w => w.Projects)
            .Include(w => w.Roles)
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
}