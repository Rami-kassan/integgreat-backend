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

    public TaskController(
    ITaskService taskService,
    IProjectService projectService,
    IWorkspaceService workspaceService)
{
    _taskService = taskService;
    _projectService = projectService;
    _workspaceService = workspaceService;
}

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetAllByProject(int projectId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        var project = await _projectService.GetByIdAsync(projectId);
        if (project == null) return NotFound();

        if (role == "ADMIN")
        {
            var tasks = await _taskService.GetAllByProjectAsync(projectId);
            return Ok(tasks);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            var tasks = await _taskService.GetAllByProjectAsync(projectId);
            return Ok(tasks);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        var task = await _taskService.GetByIdAsync(id);
        if (task == null) return NotFound();

        if (role == "ADMIN")
        {
            return Ok(task);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var project = await _projectService.GetByIdAsync(task.ProjectId);
            if (project == null) return NotFound();

            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            return Ok(task);
        }
    }

    [HttpPost]
    [SuperAdmin]
    public async Task<IActionResult> Create([FromBody] TaskRequestDto dto)
    {
        var result = await _taskService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [SuperAdmin]
    public async Task<IActionResult> Update(int id, [FromBody] TaskRequestDto dto)
    {
        var result = await _taskService.UpdateAsync(id, dto);
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
        var task = await _taskService.GetByIdAsync(id);
        if (task == null) return NotFound();

        await _taskService.DeleteAsync(id);
        return NoContent();
    }
}