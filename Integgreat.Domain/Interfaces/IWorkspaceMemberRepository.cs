using Integgreat.Domain.Entities;

namespace Integgreat.Domain.Interfaces;

public interface IWorkspaceMemberRepository
{
    Task<List<WorkspaceMember>> GetAllByWorkspaceAsync(int workspaceId);
    Task AddAsync(WorkspaceMember member);
    Task DeleteAsync(int clientId, int workspaceId);
}