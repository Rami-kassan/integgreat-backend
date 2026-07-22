using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Integgreat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
namespace Integgreat.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<string>> GetClientPermissionsAsync(int clientId)
    {
        return await _context.WorkspaceMembers
            .Where(wm => wm.ClientId == clientId)
            .SelectMany(wm => wm.Role.RolePermissions)
            .Select(rp => rp.Permission.ToString())
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<List<User>> GetRecentAsync(int count)
    {
        return await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(count)
            .ToListAsync();
    }


    public async Task<Dictionary<int, List<string>>> GetClientPermissionsByWorkspaceAsync(int clientId)
    {
        var members = await _context.WorkspaceMembers
            .Include(wm => wm.Role)
                .ThenInclude(r => r.RolePermissions)
            .Where(wm => wm.ClientId == clientId)
            .ToListAsync();

        return members.ToDictionary(
            wm => wm.WorkspaceId,
            wm => wm.Role.RolePermissions
                .Select(rp => rp.Permission.ToString())
                .ToList()
        );
    }
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

    }
}