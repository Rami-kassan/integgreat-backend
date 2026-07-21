using Integgreat.API.Helpers;
using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.Task;
using Integgreat.Application.Services;
using Integgreat.Application.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IProjectService _projectService;
    private readonly IWorkspaceService _workspaceService;
    private readonly PermissionHelper _permissionHelper;

    public TaskController(
    ITaskService taskService,
    IProjectService projectService,
    IWorkspaceService workspaceService,
    PermissionHelper permissionHelper)
    {
        _taskService = taskService;
        _projectService = projectService;
        _workspaceService = workspaceService;
        _permissionHelper = permissionHelper;
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetAllByProject(int projectId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        var project = await _projectService.GetByIdAsync(projectId);
        if (project == null) return NotFound();

        if (role == "ADMIN")
        {
            var tasks = await _taskService.GetAllByProjectAsync(projectId, true);
            return Ok(tasks);
        }

        var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
        if (!isMember) return NotFound();

        var hasViewTask = await _permissionHelper.HasPermissionAsync(User, "ViewTask", project.WorkspaceId);
        if (!hasViewTask) return Forbid();

        var canViewHours = await _permissionHelper.HasPermissionAsync(User, "ViewHours", project.WorkspaceId);
        var tasks2 = await _taskService.GetAllByProjectAsync(projectId, canViewHours);
        return Ok(tasks2);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "ADMIN")
        {
            var task = await _taskService.GetByIdAsync(id, true);
            if (task == null) return NotFound();
            return Ok(task);
        }

        var task2 = await _taskService.GetByIdAsync(id, true);
        if (task2 == null) return NotFound();

        var project = await _projectService.GetByIdAsync(task2.ProjectId);
        if (project == null) return NotFound();

        var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
        if (!isMember) return NotFound();

        var hasViewTask = await _permissionHelper.HasPermissionAsync(User, "ViewTask", project.WorkspaceId);
        if (!hasViewTask) return Forbid();

        var canViewHours = await _permissionHelper.HasPermissionAsync(User, "ViewHours", project.WorkspaceId);
        var result = await _taskService.GetByIdAsync(id, canViewHours);
        return Ok(result);
    }

    [HttpPost]
    [SuperAdmin]
    public async Task<IActionResult> Create([FromBody] TaskRequestDto dto)
    {
        var result = await _taskService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] Integgreat.Domain.Enums.TaskStatus status)
    {
        var result = await _taskService.UpdateStatusAsync(id, status);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SuperAdmin]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _taskService.GetByIdAsync(id, true);
        if (task == null) return NotFound();

        await _taskService.DeleteAsync(id);
        return NoContent();
    }
}