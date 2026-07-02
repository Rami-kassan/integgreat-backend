using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.Project;
using Integgreat.Application.Services;
using Integgreat.Application.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IWorkspaceService _workspaceService;

    public ProjectController(IProjectService projectService, IWorkspaceService workspaceService)
    {
        _projectService = projectService;
        _workspaceService = workspaceService;
    }

    [HttpGet("workspace/{workspaceId}")]
    public async Task<IActionResult> GetAllByWorkspace(int workspaceId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        var workspace = await _workspaceService.GetByIdAsync(workspaceId);
        if (workspace == null) return NotFound();

        if (role == "ADMIN")
        {
            var projects = await _projectService.GetAllByWorkspaceAsync(workspaceId);
            return Ok(projects);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst("id")!.Value);
            var isMember = await _workspaceService.IsClientMemberAsync(clientId, workspaceId);
            if (!isMember) return NotFound();

            var projects = await _projectService.GetAllByWorkspaceAsync(workspaceId);
            return Ok(projects);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "ADMIN")
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }
        else
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound();

            var clientId = int.Parse(User.FindFirst("id")!.Value);
            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            return Ok(project);
        }
    }

    [HttpPost]
    [SuperAdmin]
    public async Task<IActionResult> Create([FromBody] ProjectRequestDto dto)
    {
        try
        {
            var result = await _projectService.CreateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [SuperAdmin]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectRequestDto dto)
    {
        try
        {
            var result = await _projectService.UpdateAsync(id, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [SuperAdmin]
    public async Task<IActionResult> Delete(int id)
    {
        await _projectService.DeleteAsync(id);
        return NoContent();
    }
}