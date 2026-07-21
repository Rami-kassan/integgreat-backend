using AutoMapper;
using Integgreat.Application.DTOs.Role;
using Integgreat.Application.Exceptions;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Enums;
using Integgreat.Domain.Interfaces;

namespace Integgreat.Application.Services.Impl;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<List<RoleResponseDto>> GetAllGlobalAsync()
    {
        var roles = await _roleRepository.GetAllGlobalAsync();
        return roles.Select(MapToDto).ToList();
    }

    public async Task<List<RoleResponseDto>> GetAllByWorkspaceIncludingGlobalAsync(int workspaceId)
    {
        var roles = await _roleRepository.GetAllByWorkspaceIncludingGlobalAsync(workspaceId);
        return roles.Select(MapToDto).ToList();
    }

    public async Task<RoleResponseDto> CreateAsync(RoleRequestDto dto)
    {
        var role = new Role
        {
            Name = dto.Name,
            WorkspaceId = dto.WorkspaceId
        };
        await _roleRepository.AddAsync(role);

        foreach (var permStr in dto.Permissions)
        {
            if (Enum.TryParse<Permission>(permStr, out var perm))
            {
                role.RolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    Permission = perm
                });
            }
        }
        await _roleRepository.UpdateAsync(role);

        return MapToDto(role);
    }

    public async Task UpdatePermissionsAsync(int roleId, List<string> permissions)
    {
        var role = await _roleRepository.GetByIdWithPermissionsAsync(roleId);
        if (role == null) throw new NotFoundException("Role not found");
        if (role.WorkspaceId == null) throw new ValidationAppException("Cannot modify global roles");

        // Remplace toutes les permissions
        role.RolePermissions.Clear();
        foreach (var permStr in permissions)
        {
            if (Enum.TryParse<Permission>(permStr, out var perm))
            {
                role.RolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    Permission = perm
                });
            }
        }
        await _roleRepository.UpdateAsync(role);
    }

    private RoleResponseDto MapToDto(Role role) => new()
    {
        Id = role.Id,
        Name = role.Name,
        WorkspaceId = role.WorkspaceId,
        IsGlobal = role.WorkspaceId == null,
        Permissions = role.RolePermissions
            .Select(rp => rp.Permission.ToString())
            .ToList()
    };

    public async Task<int?> GetWorkspaceIdByRoleIdAsync(int roleId)
    {
        return await _roleRepository.GetWorkspaceIdByRoleIdAsync(roleId);
    }
}