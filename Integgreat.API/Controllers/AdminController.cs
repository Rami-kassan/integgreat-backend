using Integgreat.Application.Services;
using Integgreat.API.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("stats")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _adminService.GetStatsAsync();
        return Ok(stats);
    }

    [HttpGet("users")]
    [SuperAdmin]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _adminService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("recent-requests")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetRecentRequests()
    {
        var requests = await _adminService.GetRecentRequestsAsync();
        return Ok(requests);
    }

    [HttpGet("recent-registrations")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetRecentRegistrations()
    {
        var registrations = await _adminService.GetRecentRegistrationsAsync();
        return Ok(registrations);
    }

    [HttpGet("platform-growth")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetPlatformGrowth()
    {
        var data = await _adminService.GetPlatformGrowthAsync();
        return Ok(data);
    }

    [HttpGet("hours-summary")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetHoursSummary()
    {
        var data = await _adminService.GetHoursSummaryAsync();
        return Ok(data);
    }

    [HttpGet("hours-by-workspace")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetHoursByWorkspace()
    {
        var data = await _adminService.GetHoursByWorkspaceAsync();
        return Ok(data);
    }
}