using AutoMapper;
using Integgreat.Application.DTOs.Workspace;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;

namespace Integgreat.Application.Services.Impl;

public class WorkspaceMemberService : IWorkspaceMemberService
{
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
    private readonly IMapper _mapper;

    public WorkspaceMemberService(IWorkspaceMemberRepository workspaceMemberRepository, IMapper mapper)
    {
        _workspaceMemberRepository = workspaceMemberRepository;
        _mapper = mapper;
    }

    public async Task<List<WorkspaceMemberResponseDto>> GetAllByWorkspaceAsync(int workspaceId)
    {
        var members = await _workspaceMemberRepository.GetAllByWorkspaceAsync(workspaceId);
        return _mapper.Map<List<WorkspaceMemberResponseDto>>(members);
    }

    public async Task<WorkspaceMemberResponseDto> AddMemberAsync(WorkspaceMemberRequestDto dto)
    {
        var member = new WorkspaceMember
        {
            ClientId = dto.ClientId,
            WorkspaceId = dto.WorkspaceId,
            RoleId = dto.RoleId,
            JoinedAt = DateTime.UtcNow
        };
        await _workspaceMemberRepository.AddAsync(member);
        return _mapper.Map<WorkspaceMemberResponseDto>(member);
    }

    public async Task RemoveMemberAsync(int clientId, int workspaceId)
    {
        await _workspaceMemberRepository.DeleteAsync(clientId, workspaceId);
    }
}