using Integgreat.Application.DTOs.Task;
using Integgreat.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetAllByProject(int projectId)
    {
        var tasks = await _taskService.GetAllByProjectAsync(projectId);
        return Ok(tasks);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Create([FromBody] TaskRequestDto dto)
    {
        var result = await _taskService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}/hours")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateHours(int id, [FromBody] double completedHours)
    {
        var result = await _taskService.UpdateCompletedHoursAsync(id, completedHours);
        return Ok(result);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] Integgreat.Domain.Enums.TaskStatus status)
    {
        var result = await _taskService.UpdateStatusAsync(id, status);
        return Ok(result);
    }
}