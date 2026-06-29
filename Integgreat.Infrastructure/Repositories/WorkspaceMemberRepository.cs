using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Integgreat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Repositories;

public class WorkspaceMemberRepository : IWorkspaceMemberRepository
{
    private readonly AppDbContext _context;

    public WorkspaceMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkspaceMember>> GetAllByWorkspaceAsync(int workspaceId)
    {
        return await _context.WorkspaceMembers
            .Include(wm => wm.Client)
            .Include(wm => wm.Role)
            .Where(wm => wm.WorkspaceId == workspaceId)
            .ToListAsync();
    }

    public async Task AddAsync(WorkspaceMember member)
    {
        await _context.WorkspaceMembers.AddAsync(member);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int clientId, int workspaceId)
    {
        var member = await _context.WorkspaceMembers
            .FirstOrDefaultAsync(wm => wm.ClientId == clientId && wm.WorkspaceId == workspaceId);
        if (member != null)
        {
            _context.WorkspaceMembers.Remove(member);
            await _context.SaveChangesAsync();
        }
    }
}