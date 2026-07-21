using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.Workspace;
using Integgreat.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkspaceController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;

    public WorkspaceController(IWorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "ADMIN")
        {
            var workspaces = await _workspaceService.GetAllAsync();
            return Ok(workspaces);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var workspaces = await _workspaceService.GetAllByClientAsync(clientId);
            return Ok(workspaces);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "ADMIN")
        {
            var workspace = await _workspaceService.GetByIdAsync(id);
            if (workspace == null) return NotFound();
            return Ok(workspace);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isMember = await _workspaceService.IsClientMemberAsync(clientId, id);
            if (!isMember) return NotFound();

            var workspace = await _workspaceService.GetByIdAsync(id);
            return Ok(workspace);
        }
    }

    [HttpPost]
    [SuperAdmin]
    public async Task<IActionResult> Create([FromBody] WorkspaceRequestDto dto)
    {
        var result = await _workspaceService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [SuperAdmin]
    public async Task<IActionResult> Update(int id, [FromBody] WorkspaceRequestDto dto)
    {
        var result = await _workspaceService.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SuperAdmin]
    public async Task<IActionResult> Delete(int id)
    {
        await _workspaceService.DeleteAsync(id);
        return NoContent();
    }
}