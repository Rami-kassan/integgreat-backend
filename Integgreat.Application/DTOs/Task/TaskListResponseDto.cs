namespace Integgreat.Application.DTOs.Task;

public class TaskListResponseDto
{
    public int ProjectCompletionPct { get; set; }
    public List<object> Tasks { get; set; } = new();
}