using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.TimeEntry;
using Integgreat.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimeEntryController : ControllerBase
{
    private readonly ITimeEntryService _timeEntryService;

    public TimeEntryController(ITimeEntryService timeEntryService)
    {
        _timeEntryService = timeEntryService;
    }

    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetAllByTask(int taskId)
    {
        var entries = await _timeEntryService.GetAllByTaskAsync(taskId);
        return Ok(entries);
    }

    [HttpPost]
    [SuperAdmin]
    public async Task<IActionResult> Log([FromBody] TimeEntryRequestDto dto)
    {
        var result = await _timeEntryService.LogAsync(dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SuperAdmin]
    public async Task<IActionResult> Delete(int id)
    {
        await _timeEntryService.DeleteAsync(id);
        return NoContent();
    }
}