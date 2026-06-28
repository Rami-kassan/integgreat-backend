using Integgreat.Domain.Enums;
using TaskStatus = Integgreat.Domain.Enums.TaskStatus;

namespace Integgreat.Application.DTOs.Task;

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public int ProjectId { get; set; }
}
