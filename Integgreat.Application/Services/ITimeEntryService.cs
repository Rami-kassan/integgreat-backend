using Integgreat.Application.DTOs.TimeEntry;

namespace Integgreat.Application.Services;

public interface ITimeEntryService
{
    Task<List<TimeEntryResponseDto>> GetAllByTaskAsync(int taskId);
    Task<TimeEntryResponseDto> LogAsync(TimeEntryRequestDto dto);
    Task DeleteAsync(int id);
}