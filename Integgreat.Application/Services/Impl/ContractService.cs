using AutoMapper;
using Integgreat.Application.DTOs.Contract;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;

namespace Integgreat.Application.Services.Impl;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly IMapper _mapper;

    public ContractService(IContractRepository contractRepository, IMapper mapper)
    {
        _contractRepository = contractRepository;
        _mapper = mapper;
    }

    public async Task<List<ContractResponseDto>> GetAllByProjectAsync(int projectId)
    {
        var contracts = await _contractRepository.GetAllByProjectAsync(projectId);
        return _mapper.Map<List<ContractResponseDto>>(contracts);
    }

    public async Task<ContractResponseDto?> GetActiveByProjectAsync(int projectId)
    {
        var contract = await _contractRepository.GetActiveByProjectAsync(projectId);
        if (contract == null) return null;
        return _mapper.Map<ContractResponseDto>(contract);
    }

    public async Task<ContractResponseDto> UploadAsync(ContractRequestDto dto)
    {
        // Désactiver l'ancien contrat actif
        var activeContract = await _contractRepository.GetActiveByProjectAsync(dto.ProjectId);
        if (activeContract != null)
        {
            activeContract.IsActive = false;
            await _contractRepository.UpdateAsync(activeContract);
        }

        // Créer le nouveau contrat actif
        var contract = _mapper.Map<Contract>(dto);
        contract.IsActive = true;
        await _contractRepository.AddAsync(contract);
        return _mapper.Map<ContractResponseDto>(contract);
    }
}