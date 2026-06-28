namespace Integgreat.Application.DTOs.Contract;

public class ContractRequestDto
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public int ProjectId { get; set; }
}