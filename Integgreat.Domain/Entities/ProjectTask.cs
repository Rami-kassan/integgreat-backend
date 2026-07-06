using Integgreat.Domain.Enums;
using TaskStatus = Integgreat.Domain.Enums.TaskStatus;

namespace Integgreat.Domain.Entities;

public class ProjectTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public double EstimatedHours { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    public double CompletedHours => TimeEntries.Sum(e => e.Hours);
}