namespace Integgreat.Application.DTOs.TimeEntry;

public class TimeEntryRequestDto
{
    public double Hours { get; set; }
    public string? Note { get; set; }
    public int TaskId { get; set; }
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
}