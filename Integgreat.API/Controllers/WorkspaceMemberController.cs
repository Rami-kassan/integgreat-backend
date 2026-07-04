using Integgreat.Application.DTOs.Workspace;
using Integgreat.Application.Services;
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

    public WorkspaceMemberController(IWorkspaceMemberService workspaceMemberService)
    {
        _workspaceMemberService = workspaceMemberService;
    }

    [HttpGet("workspace/{workspaceId}")]
    public async Task<IActionResult> GetAllByWorkspace(int workspaceId)
    {
        var members = await _workspaceMemberService.GetAllByWorkspaceAsync(workspaceId);
        return Ok(members);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> AddMember([FromBody] WorkspaceMemberRequestDto dto)
    {
        var result = await _workspaceMemberService.AddMemberAsync(dto);
        return Ok(result);
    }

    [HttpDelete("client/{clientId}/workspace/{workspaceId}")]
    [Authorize(Roles = "ADMIN")]
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