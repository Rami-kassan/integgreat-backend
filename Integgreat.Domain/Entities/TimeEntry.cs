using Integgreat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

public class TimeEntry
{
    public int Id { get; set; }
    public double Hours { get; set; }
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }

    public int TaskId { get; set; }
    public ProjectTask Task { get; set; } = null!;
}