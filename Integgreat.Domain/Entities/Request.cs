using Integgreat.Domain.Enums;

namespace Integgreat.Domain.Entities;

public class Request
{
    public int Id { get; set; }
    public RequestType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}