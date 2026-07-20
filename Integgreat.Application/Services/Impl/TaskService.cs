using AutoMapper;
using Integgreat.Application.DTOs.Task;
using Integgreat.Application.Exceptions;
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

    public async Task<List<TaskResponseDto>> GetAllByProjectAsync(int projectId)
    {
        var tasks = await _taskRepository.GetAllByProjectAsync(projectId);
        return _mapper.Map<List<TaskResponseDto>>(tasks);
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return null;
        return _mapper.Map<TaskResponseDto>(task);
    }

    public async Task<TaskResponseDto> CreateAsync(TaskRequestDto dto)
    {
        var task = _mapper.Map<ProjectTask>(dto);
        await _taskRepository.AddAsync(task);
        return _mapper.Map<TaskResponseDto>(task);
    }

    public async Task<TaskResponseDto> UpdateAsync(int id, TaskRequestDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) throw new NotFoundException("Task not found");
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.EstimatedHours = dto.EstimatedHours;
        await _taskRepository.UpdateAsync(task);
        return _mapper.Map<TaskResponseDto>(task);
    }

    public async Task<TaskResponseDto> UpdateStatusAsync(int id, TaskStatus status)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) throw new NotFoundException("Task not found");
        task.Status = status;
        await _taskRepository.UpdateAsync(task);
        return _mapper.Map<TaskResponseDto>(task);
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) throw new NotFoundException("Task not found");
        await _taskRepository.DeleteAsync(id);
    }
}