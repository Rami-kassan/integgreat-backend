namespace Integgreat.Application.DTOs.Admin;

public class AdminStatsDto
{
    // Users
    public int TotalUsers { get; set; }
    public int TotalClients { get; set; }
    public int TotalAdmins { get; set; }
    public int NewUsersThisMonth { get; set; }

    // Workspaces
    public int TotalWorkspaces { get; set; }
    public int WorkspacesWithActiveProjects { get; set; }

    // Projects
    public int TotalProjects { get; set; }
    public int NewProjectsThisMonth { get; set; }
    public int ProjectsInProgress { get; set; }

    // Requests
    public int TotalOpenRequests { get; set; }
    public int TotalResolvedRequests { get; set; }

}