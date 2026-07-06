namespace Integgreat.Application.DTOs.TimeEntry;

public class TimeEntryResponseDto
{
    public int Id { get; set; }
    public double Hours { get; set; }
    public string? Note { get; set; }
    public int TaskId { get; set; }
    public DateTime LoggedAt { get; set; }
}