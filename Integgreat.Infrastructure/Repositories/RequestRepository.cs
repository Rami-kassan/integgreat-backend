using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Integgreat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Repositories;

public class RequestRepository : IRequestRepository
{
    private readonly AppDbContext _context;

    public RequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Request>> GetAllByProjectAsync(int projectId)
    {
        return await _context.Requests
            .Include(r => r.Client)
            .Where(r => r.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<Request?> GetByIdAsync(int id)
    {
        return await _context.Requests
            .Include(r => r.Client)
            .Include(r => r.Project)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Request request)
    {
        await _context.Requests.AddAsync(request);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Request request)
    {
        _context.Requests.Update(request);
        await _context.SaveChangesAsync();
    }
}