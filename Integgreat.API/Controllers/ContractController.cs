using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Integgreat.API.Helpers;
using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.Contract;
using Integgreat.Application.Services;
using Integgreat.Application.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supabase;
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
    private readonly IConfiguration _configuration;
    private readonly PermissionHelper _permissionHelper;

    public ContractController(
    IContractService contractService,
    IProjectService projectService,
    IWorkspaceService workspaceService,
    IConfiguration configuration,
    PermissionHelper permissionHelper)
    {
        _contractService = contractService;
        _projectService = projectService;
        _workspaceService = workspaceService;
        _configuration = configuration;
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
            var contracts = await _contractService.GetAllByProjectAsync(projectId);
            return Ok(contracts);
        }
        else
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var isMember = await _workspaceService.IsClientMemberAsync(clientId, project.WorkspaceId);
            if (!isMember) return NotFound();

            var hasPermission = await _permissionHelper.HasPermissionAsync(User, "ViewContract", project.WorkspaceId);
            if (!hasPermission) return Forbid();

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

            var hasPermission = await _permissionHelper.HasPermissionAsync(User, "ViewContract", project.WorkspaceId);
            if (!hasPermission) return Forbid();

            var contract = await _contractService.GetActiveByProjectAsync(projectId);
            if (contract == null) return NotFound();
            return Ok(contract);
        }
    }

    [HttpPost]
    [SuperAdmin]
    public async Task<IActionResult> Upload([FromBody] ContractRequestDto dto)
    {
        var result = await _contractService.UploadAsync(dto);
        return Ok(result);
    }

    [HttpPost("upload")]
    [SuperAdmin]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only PDF files are allowed.");

        var supabaseUrl = _configuration["Supabase:Url"]!;
        var supabaseKey = _configuration["Supabase:ServiceRoleKey"]!;
        var bucketName = _configuration["Supabase:BucketName"]!;

        var supabase = new Supabase.Client(supabaseUrl, supabaseKey);
        await supabase.InitializeAsync();

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";

        await supabase.Storage
            .From(bucketName)
            .Upload(fileBytes, fileName, new Supabase.Storage.FileOptions
            {
                ContentType = "application/pdf",
                Upsert = false,
            });

        return Ok(new
        {
            fileName = file.FileName,
            filePath = fileName
        });
    }

[HttpGet("{id}/download")]
public async Task<IActionResult> GetDownloadUrl(int id)
{
    var contract = await _contractService.GetByIdAsync(id);
    if (contract == null) return NotFound();

    var supabaseUrl = _configuration["Supabase:Url"]!;
    var supabaseKey = _configuration["Supabase:ServiceRoleKey"]!;
    var bucketName = _configuration["Supabase:BucketName"]!;

    var supabase = new Supabase.Client(supabaseUrl, supabaseKey);
    await supabase.InitializeAsync();

    var signedUrl = await supabase.Storage
        .From(bucketName)
        .CreateSignedUrl(contract.FilePath, 1800);

    return Ok(new { url = signedUrl });
}
}