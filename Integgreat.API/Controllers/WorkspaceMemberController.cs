using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.Workspace;
using Integgreat.Application.Services;
using Integgreat.Application.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkspaceMemberController : ControllerBase
{
    private readonly IWorkspaceMemberService _workspaceMemberService;
    private readonly IWorkspaceService _workspaceService;

    public WorkspaceMemberController(
        IWorkspaceMemberService workspaceMemberService,
        IWorkspaceService workspaceService)
    {
        _workspaceMemberService = workspaceMemberService;
        _workspaceService = workspaceService;
    }

    [HttpGet("workspace/{workspaceId}")]
    public async Task<IActionResult> GetAllByWorkspace(int workspaceId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isSuperAdmin = User.FindFirst("isSuperAdmin")?.Value;

        var workspace = await _workspaceService.GetByIdAsync(workspaceId);
        if (workspace == null) return NotFound();

        if (role == "ADMIN" && isSuperAdmin == "True")
        {
            var members = await _workspaceMemberService.GetAllByWorkspaceAsync(workspaceId);
            return Ok(members);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isMember = await _workspaceService.IsClientMemberAsync(clientId, workspaceId);
            if (!isMember) return NotFound();

            var members = await _workspaceMemberService.GetAllByWorkspaceAsync(workspaceId);
            return Ok(members);
        }
    }

    [HttpPost]
    [SuperAdmin]
    public async Task<IActionResult> AddMember([FromBody] WorkspaceMemberRequestDto dto)
    {
        var result = await _workspaceMemberService.AddMemberAsync(dto);
        return Ok(result);
    }

    [HttpDelete("client/{clientId}/workspace/{workspaceId}")]
    [SuperAdmin]
    public async Task<IActionResult> RemoveMember(int clientId, int workspaceId)
    {
        await _workspaceMemberService.RemoveMemberAsync(clientId, workspaceId);
        return NoContent();
    }

    [HttpPut("client/{clientId}/workspace/{workspaceId}/role")]
    [Authorize]
    public async Task<IActionResult> UpdateRole(int clientId, int workspaceId, [FromBody] int roleId)
    {
        try
        {
            // Vérifie que le user connecté est Owner de CE workspace
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isOwner = await _workspaceMemberService.IsOwnerOfWorkspaceAsync(currentUserId, workspaceId);
            if (!isOwner) return Forbid();

            await _workspaceMemberService.UpdateRoleAsync(clientId, workspaceId, roleId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}