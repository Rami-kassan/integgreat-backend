namespace Integgreat.Application.DTOs.Task;

using Integgreat.Domain.Enums;
using TaskStatus = Integgreat.Domain.Enums.TaskStatus;
public class TaskSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public int CompletionPct { get; set; }
    public int ProjectId { get; set; }
}