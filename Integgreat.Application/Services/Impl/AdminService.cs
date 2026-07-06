using Integgreat.Application.DTOs.Admin;
using Integgreat.Domain.Interfaces;

namespace Integgreat.Application.Services.Impl;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly ITaskRepository _taskRepository;

    public AdminService(
        IUserRepository userRepository,
        IWorkspaceRepository workspaceRepository,
        IProjectRepository projectRepository,
        IRequestRepository requestRepository,
        ITaskRepository taskRepository)
    {
        _userRepository = userRepository;
        _workspaceRepository = workspaceRepository;
        _projectRepository = projectRepository;
        _requestRepository = requestRepository;
        _taskRepository = taskRepository;
    }

    public async Task<AdminStatsDto> GetStatsAsync()
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var allUsers = await _userRepository.GetAllAsync();
        var totalClients = allUsers.Count(u => u is Domain.Entities.Client);
        var totalAdmins = allUsers.Count(u => u is Domain.Entities.Admin);
        var newUsersThisMonth = allUsers.Count(u => u.CreatedAt >= startOfMonth);

        var allWorkspaces = await _workspaceRepository.GetAllAsync();
        var workspacesWithActiveProjects = allWorkspaces.Count(w => w.Projects.Any());

        var allProjects = await _projectRepository.GetAllAsync();
        var newProjectsThisMonth = allProjects.Count(p => p.CreatedAt >= startOfMonth);

        var allRequests = await _requestRepository.GetAllAsync();
        var openRequests = allRequests.Count(r => r.Status == Domain.Enums.RequestStatus.Pending);
        var resolvedRequests = allRequests.Count(r => r.Status == Domain.Enums.RequestStatus.Approved);

        var allTasks = await _taskRepository.GetAllAsync();
        var projectsInProgress = allTasks
            .Where(t => t.Status == Domain.Enums.TaskStatus.InProgress)
            .Select(t => t.ProjectId)
            .Distinct()
            .Count();

        return new AdminStatsDto
        {
            TotalUsers = totalClients + totalAdmins,
            TotalClients = totalClients,
            TotalAdmins = totalAdmins,
            NewUsersThisMonth = newUsersThisMonth,
            TotalWorkspaces = allWorkspaces.Count,
            WorkspacesWithActiveProjects = workspacesWithActiveProjects,
            TotalProjects = allProjects.Count,
            NewProjectsThisMonth = newProjectsThisMonth,
            ProjectsInProgress = projectsInProgress,
            TotalOpenRequests = openRequests,
            TotalResolvedRequests = resolvedRequests,
        };
    }
    public async Task<List<AdminUserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new AdminUserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Type = u is Domain.Entities.Client ? "CLIENT" : "ADMIN",
            IsSuperAdmin = u is Domain.Entities.Admin admin && admin.IsSuperAdmin,
            CreatedAt = u.CreatedAt,
            Company = u is Domain.Entities.Client client ? client.Company : null,
            Phone = u is Domain.Entities.Client c ? c.Phone : null,
        }).ToList();
    }

    public async Task<List<AdminRecentRequestDto>> GetRecentRequestsAsync()
    {
        var requests = await _requestRepository.GetRecentAsync(5);
        return requests.Select(r => new AdminRecentRequestDto
        {
            Id = r.Id,
            Type = r.Type.ToString(),
            Description = r.Description,
            Status = r.Status.ToString(),
            CreatedAt = r.CreatedAt,
            ClientName = r.Client.Name,
            ProjectName = r.Project.Name,
        }).ToList();
    }

    public async Task<List<AdminRecentRegistrationDto>> GetRecentRegistrationsAsync()
    {
        var users = await _userRepository.GetRecentAsync(5);
        return users.Select(u => new AdminRecentRegistrationDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Type = u is Domain.Entities.Client ? "CLIENT" : "ADMIN",
            CreatedAt = u.CreatedAt,
            Company = u is Domain.Entities.Client client ? client.Company : null,
        }).ToList();
    }

    public async Task<List<AdminPlatformGrowthDto>> GetPlatformGrowthAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        var now = DateTime.UtcNow;

        var result = new List<AdminPlatformGrowthDto>();
        for (int i = 5; i >= 0; i--)
        {
            var month = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-i);
            var endOfMonth = month.AddMonths(1).AddSeconds(-1);

            // Cumulatif — tous les projets créés avant la fin de ce mois
            var count = projects.Count(p => p.CreatedAt <= endOfMonth);

            result.Add(new AdminPlatformGrowthDto
            {
                Month = month.ToString("MMM"),
                Count = count
            });
        }
        return result;
    }
    public async Task<AdminHoursSummaryDto> GetHoursSummaryAsync()
    {
        var projects = await _projectRepository.GetAllWithDetailsAsync();

        var completed = projects
            .SelectMany(p => p.Tasks)
            .SelectMany(t => t.TimeEntries)
            .Sum(te => te.Hours);

        var estimated = projects
            .SelectMany(p => p.Tasks)
            .Sum(t => t.EstimatedHours);

        var pct = estimated == 0 ? 0 : (int)Math.Round((completed / estimated) * 100);

        return new AdminHoursSummaryDto
        {
            TotalCompletedHours = completed,
            TotalEstimatedHours = estimated,
            DeliveredPct = pct
        };
    }

    public async Task<List<AdminWorkspaceHoursDto>> GetHoursByWorkspaceAsync()
    {
        var projects = await _projectRepository.GetAllWithDetailsAsync();

        return projects
            .GroupBy(p => p.Workspace.Name)
            .Select(g => new AdminWorkspaceHoursDto
            {
                WorkspaceName = g.Key,
                CompletedHours = g.SelectMany(p => p.Tasks)
                                  .SelectMany(t => t.TimeEntries)
                                  .Sum(te => te.Hours),
                EstimatedHours = g.SelectMany(p => p.Tasks)
                                  .Sum(t => t.EstimatedHours),
            })
            .OrderByDescending(w => w.CompletedHours)
            .ToList();
    }
}