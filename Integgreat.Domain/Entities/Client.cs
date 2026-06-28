namespace Integgreat.Domain.Entities;

public class Client : User
{
    public string Company { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;

    public ICollection<WorkspaceMember> WorkspaceMembers { get; set; } = new List<WorkspaceMember>();
    public ICollection<Request> Requests { get; set; } = new List<Request>();
}