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
}