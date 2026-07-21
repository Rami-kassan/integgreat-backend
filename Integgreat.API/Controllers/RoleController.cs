using Integgreat.API.Helpers;
using Integgreat.Application.DTOs.Role;
using Integgreat.Application.Services;
using Integgreat.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly PermissionHelper _permissionHelper;

    public RoleController(IRoleService roleService, PermissionHelper permissionHelper)
    {
        _roleService = roleService;
        _permissionHelper = permissionHelper;
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
        try
        {
            var hasPermission = await _permissionHelper.HasPermissionAsync(User, "ManageMembers", dto.WorkspaceId);
            if (!hasPermission) return Forbid();

            var result = await _roleService.CreateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/permissions")]
    [Authorize]
    public async Task<IActionResult> UpdatePermissions(int id, [FromBody] List<string> permissions)
    {
        try
        {
            var workspaceId = await _roleService.GetWorkspaceIdByRoleIdAsync(id);
            if (workspaceId == null) return NotFound();

            var hasPermission = await _permissionHelper.HasPermissionAsync(User, "ManageMembers", workspaceId.Value);
            if (!hasPermission) return Forbid();

            await _roleService.UpdatePermissionsAsync(id, permissions);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}