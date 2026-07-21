using Integgreat.Application.DTOs.Task;
using TaskStatus = Integgreat.Domain.Enums.TaskStatus;

namespace Integgreat.Application.Services;

public interface ITaskService
{
    Task<TaskListResponseDto> GetAllByProjectAsync(int projectId, bool canViewHours);
    Task<TaskResponseDto?> GetByIdAsync(int id, bool canViewHours);
    Task<TaskResponseDto> CreateAsync(TaskRequestDto dto);
    Task<TaskResponseDto> UpdateAsync(int id, TaskRequestDto dto);
    Task<TaskResponseDto> UpdateStatusAsync(int id, TaskStatus status);
    Task DeleteAsync(int id);
}