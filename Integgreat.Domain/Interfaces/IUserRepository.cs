using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<List<string>> GetClientPermissionsAsync(int clientId);
}