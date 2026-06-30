using AutoMapper;
using Integgreat.Application.DTOs.Workspace;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Integgreat.Application.Services.Impl;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
    private readonly IMapper _mapper;

    public WorkspaceService(
        IWorkspaceRepository workspaceRepository,
        IRoleRepository roleRepository,
        IWorkspaceMemberRepository workspaceMemberRepository,
        IMapper mapper)
    {
        _workspaceRepository = workspaceRepository;
        _roleRepository = roleRepository;
        _workspaceMemberRepository = workspaceMemberRepository;
        _mapper = mapper;
    }

    public async Task<List<WorkspaceResponseDto>> GetAllAsync()
    {
        var workspaces = await _workspaceRepository.GetAllAsync();
        return _mapper.Map<List<WorkspaceResponseDto>>(workspaces);
    }

    public async Task<WorkspaceResponseDto?> GetByIdAsync(int id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return null;
        return _mapper.Map<WorkspaceResponseDto>(workspace);
    }

    public async Task<WorkspaceResponseDto> CreateAsync(WorkspaceRequestDto dto)
    {
        var workspace = new Workspace
        {
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow
        };
        await _workspaceRepository.AddAsync(workspace);

        var ownerRole = await _roleRepository.GetByNameAsync("Owner");

        var member = new WorkspaceMember
        {
            ClientId = dto.ClientId,
            WorkspaceId = workspace.Id,
            RoleId = ownerRole!.Id,
            JoinedAt = DateTime.UtcNow
        };
        await _workspaceMemberRepository.AddAsync(member);

        return _mapper.Map<WorkspaceResponseDto>(workspace);
    }

    public async Task<WorkspaceResponseDto> UpdateAsync(int id, WorkspaceRequestDto dto)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) throw new Exception("Workspace not found");
        _mapper.Map(dto, workspace);
        await _workspaceRepository.UpdateAsync(workspace);
        return _mapper.Map<WorkspaceResponseDto>(workspace);
    }

    public async Task DeleteAsync(int id)
    {
        await _workspaceRepository.DeleteAsync(id);
    }
}