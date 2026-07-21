using AutoMapper;
using Integgreat.Application.DTOs.Task;
using Integgreat.Application.DTOs.TimeEntry;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using TaskStatus = Integgreat.Domain.Enums.TaskStatus;

namespace Integgreat.Application.Services.Impl;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<TaskListResponseDto> GetAllByProjectAsync(int projectId, bool canViewHours)
    {
        var tasks = await _taskRepository.GetAllByProjectAsync(projectId);

        var totalEstimated = tasks.Sum(t => t.EstimatedHours);
        var totalCompleted = tasks.SelectMany(t => t.TimeEntries).Sum(te => te.Hours);
        var projectPct = totalEstimated == 0 ? 0 : (int)Math.Round((totalCompleted / totalEstimated) * 100);

        if (canViewHours)
        {
            var dtos = _mapper.Map<List<TaskResponseDto>>(tasks);
            return new TaskListResponseDto
            {
                ProjectCompletionPct = projectPct,
                Tasks = dtos.Cast<object>().ToList(),
            };
        }
        else
        {
            var dtos = _mapper.Map<List<TaskSummaryDto>>(tasks);
            return new TaskListResponseDto
            {
                ProjectCompletionPct = projectPct,
                Tasks = dtos.Cast<object>().ToList(),
            };
        }
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int id, bool canViewHours)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return null;
        var dto = _mapper.Map<TaskResponseDto>(task);

        if (!canViewHours)
        {
            dto.CompletedHours = 0;
            dto.EstimatedHours = 0;
            dto.TimeEntries = new List<TimeEntryResponseDto>();
        }
        return dto;
    }

    public async Task<TaskResponseDto> CreateAsync(TaskRequestDto dto)
    {
        var task = _mapper.Map<ProjectTask>(dto);
        await _taskRepository.AddAsync(task);
        return _mapper.Map<TaskResponseDto>(task);
    }

    public async Task<TaskResponseDto> UpdateStatusAsync(int id, TaskStatus status)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) throw new Exception("Task not found");
        task.Status = status;
        await _taskRepository.UpdateAsync(task);
        return _mapper.Map<TaskResponseDto>(task);
    }

    public async Task DeleteAsync(int id)
    {
        await _taskRepository.DeleteAsync(id);
    }
}