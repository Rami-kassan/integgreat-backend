using Integgreat.Application.DTOs.Admin;

namespace Integgreat.Application.Services;

public interface IAdminService
{
    Task<AdminStatsDto> GetStatsAsync();
    Task<List<AdminPlatformGrowthDto>> GetPlatformGrowthAsync();
    Task<List<AdminUserDto>> GetAllUsersAsync();
    Task<List<AdminRecentRequestDto>> GetRecentRequestsAsync();
    Task<List<AdminRecentRegistrationDto>> GetRecentRegistrationsAsync();
    Task<AdminHoursSummaryDto> GetHoursSummaryAsync();
    Task<List<AdminWorkspaceHoursDto>> GetHoursByWorkspaceAsync();
    Task<AdminUserDetailsDto?> GetUserDetailsAsync(int userId);
    Task<List<AdminWorkspaceDto>> GetWorkspacesAsync();
    Task<AdminWorkspaceDetailsDto?> GetWorkspaceDetailsAsync(int workspaceId);
    Task<List<AdminProjectDto>> GetProjectsAsync();
    Task<List<AdminTaskDto>> GetAllTasksAsync();
    Task<List<AdminRequestDto>> GetAllRequestsAsync();
    Task<List<AdminContractDto>> GetAllContractsAsync();
}