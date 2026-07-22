using AutoMapper;
using Integgreat.Application.DTOs.Workspace;
using Integgreat.Application.Exceptions;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;

namespace Integgreat.Application.Services.Impl;

public class WorkspaceMemberService : IWorkspaceMemberService
{
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public WorkspaceMemberService(
        IWorkspaceMemberRepository workspaceMemberRepository,
        IRoleRepository roleRepository,
        IMapper mapper)
    {
        _workspaceMemberRepository = workspaceMemberRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<List<WorkspaceMemberResponseDto>> GetAllByWorkspaceAsync(int workspaceId)
    {
        var members = await _workspaceMemberRepository.GetAllByWorkspaceAsync(workspaceId);
        return _mapper.Map<List<WorkspaceMemberResponseDto>>(members);
    }

    public async Task<WorkspaceMemberResponseDto> AddMemberAsync(WorkspaceMemberRequestDto dto)
    {
        // Un workspace ne peut avoir qu'un seul Owner (sauf s'il n'en a plus, ex: ancien Owner supprimé).
        var role = await _roleRepository.GetByIdAsync(dto.RoleId);
        if (role?.Name == "Owner")
        {
            var existingMembers = await _workspaceMemberRepository.GetAllByWorkspaceAsync(dto.WorkspaceId);
            if (existingMembers.Any(m => m.Role.Name == "Owner"))
                throw new ValidationAppException("This workspace already has an Owner.");
        }

        var member = new WorkspaceMember
        {
            ClientId = dto.ClientId,
            WorkspaceId = dto.WorkspaceId,
            RoleId = dto.RoleId,
            JoinedAt = DateTime.UtcNow
        };
        await _workspaceMemberRepository.AddAsync(member);

        // Recharge le membre avec les navigation properties
        var memberWithDetails = await _workspaceMemberRepository.GetByClientAndWorkspaceAsync(dto.ClientId, dto.WorkspaceId);
        return _mapper.Map<WorkspaceMemberResponseDto>(memberWithDetails);
    }

    public async Task RemoveMemberAsync(int clientId, int workspaceId)
    {
        var member = await _workspaceMemberRepository.GetByClientAndWorkspaceAsync(clientId, workspaceId);
        if (member == null) throw new NotFoundException("Member not found");
        await _workspaceMemberRepository.DeleteAsync(clientId, workspaceId);
    }

    public async Task UpdateRoleAsync(int clientId, int workspaceId, int roleId)
    {
        var member = await _workspaceMemberRepository.GetByClientAndWorkspaceAsync(clientId, workspaceId);
        if (member == null) throw new NotFoundException("Member not found");

        // Vérifie si le nouveau rôle est Owner
        var newRole = await _roleRepository.GetByIdAsync(roleId);
        if (newRole?.Name == "Owner")
            throw new ValidationAppException("Cannot assign Owner role — a workspace can only have one Owner.");

        member.RoleId = roleId;
        await _workspaceMemberRepository.UpdateAsync(member);
    }

    public async Task<bool> IsOwnerOfWorkspaceAsync(int clientId, int workspaceId)
    {
        var member = await _workspaceMemberRepository.GetByClientAndWorkspaceAsync(clientId, workspaceId);
        return member?.Role.Name == "Owner";
    }
}