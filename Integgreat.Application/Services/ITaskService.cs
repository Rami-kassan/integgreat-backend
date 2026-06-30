using Integgreat.Application.DTOs.Task;
using TaskStatus = Integgreat.Domain.Enums.TaskStatus;

namespace Integgreat.Application.Services;

public interface ITaskService
{
    Task<List<TaskResponseDto>> GetAllByProjectAsync(int projectId);
    Task<TaskResponseDto?> GetByIdAsync(int id);
    Task<TaskResponseDto> CreateAsync(TaskRequestDto dto);
    Task<TaskResponseDto> UpdateCompletedHoursAsync(int id, double completedHours);
    Task<TaskResponseDto> UpdateStatusAsync(int id, TaskStatus status);
    Task DeleteAsync(int id);
}