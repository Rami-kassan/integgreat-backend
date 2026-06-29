using Integgreat.Application.DTOs.Contract;
using Integgreat.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetAllByProject(int projectId)
    {
        var contracts = await _contractService.GetAllByProjectAsync(projectId);
        return Ok(contracts);
    }

    [HttpGet("project/{projectId}/active")]
    public async Task<IActionResult> GetActive(int projectId)
    {
        var contract = await _contractService.GetActiveByProjectAsync(projectId);
        if (contract == null) return NotFound();
        return Ok(contract);
    }

    [HttpPost]
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