using Integgreat.Application.DTOs.Workspace;
using Integgreat.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}