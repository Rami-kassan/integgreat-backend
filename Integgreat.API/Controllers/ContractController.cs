using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.Contract;
using Integgreat.Application.Services;
using Integgreat.Application.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly IProjectService _projectService;
    private readonly IWorkspaceService _workspaceService;

    public ContractController(
    IContractService contractService,
    IProjectService projectService,
    IWorkspaceService workspaceService)
    {
        _contractService = contractService;
        _projectService = projectService;
        _workspaceService = workspaceService;
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
            var contracts = await _contractService.GetAllByProjectAsync(projectId);
            return Ok(contracts);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            var contracts = await _contractService.GetAllByProjectAsync(projectId);
            return Ok(contracts);
        }
    }

    [HttpGet("project/{projectId}/active")]
    public async Task<IActionResult> GetActive(int projectId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isSuperAdmin = User.FindFirst("isSuperAdmin")?.Value;

        // Vérifie que le projet existe
        var project = await _projectService.GetByIdAsync(projectId);
        if (project == null) return NotFound();

        if (role == "ADMIN" && isSuperAdmin == "True")
        {
            var contract = await _contractService.GetActiveByProjectAsync(projectId);
            if (contract == null) return NotFound();
            return Ok(contract);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            var contract = await _contractService.GetActiveByProjectAsync(projectId);
            if (contract == null) return NotFound();
            return Ok(contract);
        }
    }

    [HttpPost]
    [SuperAdmin]
    public async Task<IActionResult> Upload([FromBody] ContractRequestDto dto)
    {
        try
        {
            var result = await _contractService.UploadAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}