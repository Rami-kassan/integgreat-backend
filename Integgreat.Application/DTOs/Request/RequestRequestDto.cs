using Integgreat.Domain.Enums;

namespace Integgreat.Application.DTOs.Request;

public class RequestRequestDto
{
    public RequestType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public int ClientId { get; set; }
}