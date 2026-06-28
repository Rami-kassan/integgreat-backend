using Integgreat.Application.DTOs.Contract;
using Integgreat.Application.DTOs.Task;

namespace Integgreat.Application.DTOs.Project;

public class ProjectResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal HoursWorked { get; set; }
    public DateTime CreatedAt { get; set; }
    public int WorkspaceId { get; set; }
    public List<ContractResponseDto> Contracts { get; set; } = new();
    public List<TaskResponseDto> Tasks { get; set; } = new();
}