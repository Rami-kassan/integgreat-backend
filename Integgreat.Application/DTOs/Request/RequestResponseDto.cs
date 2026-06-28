using Integgreat.Domain.Enums;

namespace Integgreat.Application.DTOs.Request;

public class RequestResponseDto
{
    public int Id { get; set; }
    public RequestType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public RequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProjectId { get; set; }
    public int ClientId { get; set; }
}