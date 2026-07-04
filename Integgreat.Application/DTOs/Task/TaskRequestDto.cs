using Integgreat.Domain.Enums;

namespace Integgreat.Application.DTOs.Task;

public class TaskRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public double EstimatedHours { get; set; }
}