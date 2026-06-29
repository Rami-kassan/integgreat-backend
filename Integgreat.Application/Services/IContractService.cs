using Integgreat.Application.DTOs.Contract;

namespace Integgreat.Application.Services;

public interface IContractService
{
    Task<List<ContractResponseDto>> GetAllByProjectAsync(int projectId);
    Task<ContractResponseDto?> GetActiveByProjectAsync(int projectId);
    Task<ContractResponseDto> UploadAsync(ContractRequestDto dto);
}