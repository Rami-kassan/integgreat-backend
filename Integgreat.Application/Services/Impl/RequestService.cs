using AutoMapper;
using Integgreat.Application.DTOs.Request;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Enums;
using Integgreat.Domain.Interfaces;

namespace Integgreat.Application.Services.Impl;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public RequestService(IRequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<List<RequestResponseDto>> GetAllByProjectAsync(int projectId)
    {
        var requests = await _requestRepository.GetAllByProjectAsync(projectId);
        return _mapper.Map<List<RequestResponseDto>>(requests);
    }

    public async Task<RequestResponseDto?> GetByIdAsync(int id)
    {
        var request = await _requestRepository.GetByIdAsync(id);
        if (request == null) return null;
        return _mapper.Map<RequestResponseDto>(request);
    }

    public async Task<RequestResponseDto> CreateAsync(RequestRequestDto dto)
    {
        var request = _mapper.Map<Request>(dto);
        await _requestRepository.AddAsync(request);
        return _mapper.Map<RequestResponseDto>(request);
    }

    public async Task<RequestResponseDto> UpdateStatusAsync(int id, RequestStatus status)
    {
        var request = await _requestRepository.GetByIdAsync(id);
        if (request == null) throw new Exception("Request not found");
        request.Status = status;
        await _requestRepository.UpdateAsync(request);
        return _mapper.Map<RequestResponseDto>(request);
    }
}