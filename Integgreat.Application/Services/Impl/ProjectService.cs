using AutoMapper;
using Integgreat.Application.DTOs.Project;
using Integgreat.Application.Exceptions;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
namespace Integgreat.Application.Services.Impl;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
    private readonly IMapper _mapper;

    public ProjectService(
    IProjectRepository projectRepository,
    IWorkspaceRepository workspaceRepository,
    IWorkspaceMemberRepository workspaceMemberRepository,
    IMapper mapper)
    {
        _projectRepository = projectRepository;
        _workspaceRepository = workspaceRepository;
        _workspaceMemberRepository = workspaceMemberRepository;
        _mapper = mapper;
    }

    public async Task<List<ProjectResponseDto>> GetAllByWorkspaceAsync(int workspaceId)
    {
        var projects = await _projectRepository.GetAllByWorkspaceAsync(workspaceId);
        return _mapper.Map<List<ProjectResponseDto>>(projects);
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return null;

        var dto = _mapper.Map<ProjectResponseDto>(project);
        dto.TotalHoursWorked = project.Tasks.Sum(t => t.CompletedHours);

        return dto;
    }

    public async Task<ProjectResponseDto> CreateAsync(ProjectRequestDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            WorkspaceId = dto.WorkspaceId,
            CreatedAt = DateTime.UtcNow
        };
        await _projectRepository.AddAsync(project);

        return _mapper.Map<ProjectResponseDto>(project);
    }

    public async Task<ProjectResponseDto> UpdateAsync(int id, ProjectRequestDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) throw new NotFoundException("Project not found");
        _mapper.Map(dto, project);
        await _projectRepository.UpdateAsync(project);
        return _mapper.Map<ProjectResponseDto>(project);
    }

    public async Task DeleteAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) throw new NotFoundException("Project not found");
        await _projectRepository.DeleteAsync(id);
    }
}