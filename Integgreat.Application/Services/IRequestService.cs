using Integgreat.Application.DTOs.Request;
using Integgreat.Domain.Enums;

namespace Integgreat.Application.Services;

public interface IRequestService
{
    Task<List<RequestResponseDto>> GetAllByProjectAsync(int projectId);
    Task<RequestResponseDto?> GetByIdAsync(int id);
    Task<RequestResponseDto> CreateAsync(RequestRequestDto dto);
    Task<RequestResponseDto> UpdateStatusAsync(int id, RequestStatus status);
}