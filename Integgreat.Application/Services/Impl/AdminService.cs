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
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;

    public AdminService(
        IUserRepository userRepository,
        IWorkspaceRepository workspaceRepository,
        IProjectRepository projectRepository,
        IRequestRepository requestRepository,
        ITaskRepository taskRepository,
        IWorkspaceMemberRepository workspaceMemberRepository)
    {
        _userRepository = userRepository;
        _workspaceRepository = workspaceRepository;
        _projectRepository = projectRepository;
        _requestRepository = requestRepository;
        _taskRepository = taskRepository;
        _workspaceMemberRepository = workspaceMemberRepository;
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

    public async Task<AdminUserDetailsDto?> GetUserDetailsAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return null;

        var workspaceMembers = await _workspaceMemberRepository.GetAllByClientAsync(userId);

        var workspaces = workspaceMembers.Select(wm => new AdminUserWorkspaceDto
        {
            Id = wm.Workspace.Id,
            Name = wm.Workspace.Name,
            ProjectCount = wm.Workspace.Projects.Count,
            MemberCount = wm.Workspace.Members.Count,
            CreatedAt = wm.Workspace.CreatedAt,
        }).ToList();

        var projects = workspaceMembers
            .SelectMany(wm => wm.Workspace.Projects)
            .Select(p => {
                var estimated = p.Tasks.Sum(t => t.EstimatedHours);
                var completed = p.Tasks.SelectMany(t => t.TimeEntries).Sum(te => te.Hours);
                var pct = estimated == 0 ? 0 : (int)Math.Round((completed / estimated) * 100);
                return new AdminUserProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    EstimatedHours = estimated,
                    CompletedHours = completed,
                    TaskCount = p.Tasks.Count,
                    DoneTaskCount = p.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Done),
                    Pct = pct,
                };
            }).ToList();

        return new AdminUserDetailsDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Type = user is Domain.Entities.Client ? "CLIENT" : "ADMIN",
            IsSuperAdmin = user is Domain.Entities.Admin admin && admin.IsSuperAdmin,
            CreatedAt = user.CreatedAt,
            Company = user is Domain.Entities.Client client ? client.Company : null,
            Phone = user is Domain.Entities.Client c ? c.Phone : null,
            Workspaces = workspaces,
            Projects = projects,
        };
    }
    public async Task<List<AdminWorkspaceDto>> GetWorkspacesAsync()
    {
        var workspaces = await _workspaceRepository.GetAllWithDetailsAsync();

        return workspaces.Select(w =>
        {
            var totalEstimated = w.Projects
                .SelectMany(p => p.Tasks)
                .Sum(t => t.EstimatedHours);

            var totalCompleted = w.Projects
                .SelectMany(p => p.Tasks)
                .SelectMany(t => t.TimeEntries)
                .Sum(te => te.Hours);

            var pct = totalEstimated == 0 ? 0 :
                (int)Math.Round((totalCompleted / totalEstimated) * 100);

            var totalTasks = w.Projects
                .SelectMany(p => p.Tasks)
                .Count();

            var owner = w.Members
                .FirstOrDefault(m => m.Role.Name == "Owner");

            return new AdminWorkspaceDto
            {
                Id = w.Id,
                Name = w.Name,
                CreatedAt = w.CreatedAt,
                ProjectCount = w.Projects.Count,
                TotalTaskCount = totalTasks,
                MemberCount = w.Members.Count,
                CompletionPct = pct,
                OwnerName = owner?.Client.Name ?? "—",
                OwnerEmail = owner?.Client.Email ?? "—",
            };
        }).ToList();
    }

    public async Task<AdminWorkspaceDetailsDto?> GetWorkspaceDetailsAsync(int workspaceId)
    {
        var w = await _workspaceRepository.GetByIdWithDetailsAsync(workspaceId);
        if (w == null) return null;

        var totalEstimated = w.Projects.SelectMany(p => p.Tasks).Sum(t => t.EstimatedHours);
        var totalCompleted = w.Projects.SelectMany(p => p.Tasks).SelectMany(t => t.TimeEntries).Sum(te => te.Hours);
        var pct = totalEstimated == 0 ? 0 : (int)Math.Round((totalCompleted / totalEstimated) * 100);

        var projects = w.Projects.Select(p =>
        {
            var estimated = p.Tasks.Sum(t => t.EstimatedHours);
            var completed = p.Tasks.SelectMany(t => t.TimeEntries).Sum(te => te.Hours);
            var projectPct = estimated == 0 ? 0 : (int)Math.Round((completed / estimated) * 100);

            return new AdminWorkspaceProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                CompletionPct = projectPct,
                CompletedHours = completed,
                EstimatedHours = estimated,
                DoneTaskCount = p.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Done),
                InProgressTaskCount = p.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.InProgress),
                TodoTaskCount = p.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Todo),
            };
        }).ToList();

        var members = w.Members.Select(m => new AdminWorkspaceMemberDto
        {
            ClientId = m.ClientId,
            Name = m.Client.Name,
            Email = m.Client.Email,
            RoleName = m.Role.Name,
        }).ToList();

        return new AdminWorkspaceDetailsDto
        {
            Id = w.Id,
            Name = w.Name,
            CreatedAt = w.CreatedAt,
            ProjectCount = w.Projects.Count,
            TotalTaskCount = w.Projects.SelectMany(p => p.Tasks).Count(),
            TotalCompletedHours = totalCompleted,
            TotalEstimatedHours = totalEstimated,
            MemberCount = w.Members.Count,
            CompletionPct = pct,
            Projects = projects,
            Members = members,
        };
    }

    public async Task<List<AdminProjectDto>> GetProjectsAsync()
    {
        var projects = await _projectRepository.GetAllWithDetailsAsync();

        return projects.Select(p =>
        {
            var estimated = p.Tasks.Sum(t => t.EstimatedHours);
            var completed = p.Tasks.SelectMany(t => t.TimeEntries).Sum(te => te.Hours);
            var pct = estimated == 0 ? 0 : (int)Math.Round((completed / estimated) * 100);

            return new AdminProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                WorkspaceId = p.WorkspaceId,
                WorkspaceName = p.Workspace.Name,
                CreatedAt = p.CreatedAt,
                CompletionPct = pct,
                CompletedHours = completed,
                EstimatedHours = estimated,
                TaskCount = p.Tasks.Count,
                DoneTaskCount = p.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Done),
            };
        }).ToList();
    }
    public async Task<List<AdminRequestDto>> GetAllRequestsAsync()
    {
        var requests = await _requestRepository.GetAllWithDetailsAsync();

        return requests.Select(r => new AdminRequestDto
        {
            Id = r.Id,
            Type = r.Type.ToString(),
            Description = r.Description,
            Status = r.Status.ToString(),
            CreatedAt = r.CreatedAt,
            ClientId = r.ClientId,
            ClientName = r.Client.Name,
            ClientEmail = r.Client.Email,
            ProjectId = r.ProjectId,
            ProjectName = r.Project.Name,
            WorkspaceName = r.Project.Workspace.Name,
        }).ToList();
    }
}