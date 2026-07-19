using Integgreat.Application.DTOs.Role;
using Integgreat.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAllGlobalAsync();
        return Ok(roles);
    }

    [HttpGet("workspace/{workspaceId}")]
    public async Task<IActionResult> GetAllByWorkspace(int workspaceId)
    {
        var roles = await _roleService.GetAllByWorkspaceIncludingGlobalAsync(workspaceId);
        return Ok(roles);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] RoleRequestDto dto)
    {
        var result = await _roleService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}/permissions")]
    [Authorize]
    public async Task<IActionResult> UpdatePermissions(int id, [FromBody] List<string> permissions)
    {
        await _roleService.UpdatePermissionsAsync(id, permissions);
        return Ok();
    }
}