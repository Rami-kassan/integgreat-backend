namespace Integgreat.Application.DTOs.Auth;

public class ClientRegisterDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
}