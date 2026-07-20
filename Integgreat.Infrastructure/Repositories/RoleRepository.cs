using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Integgreat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Name == name && r.WorkspaceId == null);
    }

    public async Task<List<Role>> GetAllByWorkspaceAsync(int? workspaceId)
    {
        return await _context.Roles
            .Where(r => r.WorkspaceId == workspaceId || r.WorkspaceId == null)
            .Include(r => r.RolePermissions)
            .ToListAsync();
    }

    public async Task AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Role>> GetAllGlobalAsync()
    {
        return await _context.Roles
            .Where(r => r.WorkspaceId == null)
            .ToListAsync();
    }

    public async Task<List<Role>> GetAllByWorkspaceIncludingGlobalAsync(int workspaceId)
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .Where(r => r.WorkspaceId == null || r.WorkspaceId == workspaceId)
            .ToListAsync();
    }

    public async Task<Role?> GetByIdWithPermissionsAsync(int id)
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }

    public async Task<int?> GetWorkspaceIdByRoleIdAsync(int roleId)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId);
        return role?.WorkspaceId;
    }
}