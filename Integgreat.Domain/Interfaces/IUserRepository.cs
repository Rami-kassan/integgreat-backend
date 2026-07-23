using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<List<string>> GetClientPermissionsAsync(int clientId);
    Task<List<User>> GetAllAsync();
    Task<List<User>> GetRecentAsync(int count);

    Task<Dictionary<int, List<string>>> GetClientPermissionsByWorkspaceAsync(int clientId);
    Task UpdateAsync(User user);

    Task DeleteAsync(int id);

    Task<User?> GetByResetTokenAsync(string token);
}