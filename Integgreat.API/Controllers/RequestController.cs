using Integgreat.API.Helpers;
using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.Request;
using Integgreat.Application.Services;
using Integgreat.Application.Services.Impl;
using Integgreat.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IProjectService _projectService;
    private readonly IWorkspaceService _workspaceService;
    private readonly PermissionHelper _permissionHelper;

    public RequestController(
        IRequestService requestService,
        IProjectService projectService,
        IWorkspaceService workspaceService,
        PermissionHelper permissionHelper)
    {
        _requestService = requestService;
        _projectService = projectService;
        _workspaceService = workspaceService;
        _permissionHelper = permissionHelper;
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetAllByProject(int projectId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isSuperAdmin = User.FindFirst("isSuperAdmin")?.Value;

        var project = await _projectService.GetByIdAsync(projectId);
        if (project == null) return NotFound();

        if (role == "ADMIN" && isSuperAdmin == "True")
        {
            var requests = await _requestService.GetAllByProjectAsync(projectId);
            return Ok(requests);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            var hasPermission = await _permissionHelper.HasPermissionAsync(User, "ViewRequest", project.WorkspaceId);
            if (!hasPermission) return Forbid();

            var requests = await _requestService.GetAllByProjectAsync(projectId);
            return Ok(requests);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isSuperAdmin = User.FindFirst("isSuperAdmin")?.Value;

        var request = await _requestService.GetByIdAsync(id);
        if (request == null) return NotFound();

        if (role == "ADMIN" && isSuperAdmin == "True")
        {
            return Ok(request);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var project = await _projectService.GetByIdAsync(request.ProjectId);
            if (project == null) return NotFound();

            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            var hasPermission = await _permissionHelper.HasPermissionAsync(User, "ViewRequest", project.WorkspaceId);
            if (!hasPermission) return Forbid();

            return Ok(request);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RequestRequestDto dto)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isSuperAdmin = User.FindFirst("isSuperAdmin")?.Value;

        // Vérifie que le projet existe
        var project = await _projectService.GetByIdAsync(dto.ProjectId);
        if (project == null) return NotFound();

        // SuperAdmin peut toujours créer
        if (role != "ADMIN" || isSuperAdmin != "True")
        {
            // Client doit être membre du workspace
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            var hasPermission = await _permissionHelper.HasPermissionAsync(User, "CreateRequest", project.WorkspaceId);
            if (!hasPermission) return Forbid();
        }

        try
        {
            var result = await _requestService.CreateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/status")]
    [SuperAdmin]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] RequestStatus status)
    {
        try
        {
            var result = await _requestService.UpdateStatusAsync(id, status);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}