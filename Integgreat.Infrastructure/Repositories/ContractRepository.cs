using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Integgreat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly AppDbContext _context;

    public ContractRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Contract>> GetAllByProjectAsync(int projectId)
    {
        return await _context.Contracts
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.UploadedAt)
            .ToListAsync();
    }

    public async Task<Contract?> GetActiveByProjectAsync(int projectId)
    {
        return await _context.Contracts
            .Where(c => c.ProjectId == projectId && c.IsActive)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contract contract)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Contract>> GetAllWithDetailsAsync()
    {
        return await _context.Contracts
            .Include(c => c.Project)
                .ThenInclude(p => p.Workspace)
                    .ThenInclude(w => w.Members)
                        .ThenInclude(m => m.Client)
            .Include(c => c.Project)
                .ThenInclude(p => p.Workspace)
                    .ThenInclude(w => w.Members)
                        .ThenInclude(m => m.Role)
            .OrderByDescending(c => c.UploadedAt)
            .ToListAsync();
    }
}