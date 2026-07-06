using AutoMapper;
using Integgreat.Application.DTOs.TimeEntry;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;

namespace Integgreat.Application.Services.Impl;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IMapper _mapper;

    public TimeEntryService(ITimeEntryRepository timeEntryRepository, IMapper mapper)
    {
        _timeEntryRepository = timeEntryRepository;
        _mapper = mapper;
    }

    public async Task<List<TimeEntryResponseDto>> GetAllByTaskAsync(int taskId)
    {
        var entries = await _timeEntryRepository.GetAllByTaskAsync(taskId);
        return _mapper.Map<List<TimeEntryResponseDto>>(entries);
    }

    public async Task<TimeEntryResponseDto> LogAsync(TimeEntryRequestDto dto)
    {
        var timeEntry = _mapper.Map<TimeEntry>(dto);
        await _timeEntryRepository.AddAsync(timeEntry);
        return _mapper.Map<TimeEntryResponseDto>(timeEntry);
    }

    public async Task DeleteAsync(int id)
    {
        await _timeEntryRepository.DeleteAsync(id);
    }
}