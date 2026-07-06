namespace Integgreat.Application.DTOs.Admin;

public class AdminHoursSummaryDto
{
    public double TotalCompletedHours { get; set; }
    public double TotalEstimatedHours { get; set; }
    public int DeliveredPct { get; set; }
}