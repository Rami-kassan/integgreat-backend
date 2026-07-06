using AutoMapper;
using Integgreat.Application.DTOs.Auth;
using Integgreat.Application.DTOs.Contract;
using Integgreat.Application.DTOs.Project;
using Integgreat.Application.DTOs.Request;
using Integgreat.Application.DTOs.Role;
using Integgreat.Application.DTOs.Task;
using Integgreat.Application.DTOs.TimeEntry;
using Integgreat.Application.DTOs.Workspace;
using Integgreat.Domain.Entities;

namespace Integgreat.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Project
        CreateMap<Project, ProjectResponseDto>();
        CreateMap<ProjectRequestDto, Project>();

        // Contract
        CreateMap<Contract, ContractResponseDto>();
        CreateMap<ContractRequestDto, Contract>();

        // Task
        CreateMap<ProjectTask, TaskResponseDto>();
        CreateMap<TaskRequestDto, ProjectTask>();

        // Request
        CreateMap<Request, RequestResponseDto>();
        CreateMap<RequestRequestDto, Request>();

        // Workspace
        CreateMap<Workspace, WorkspaceResponseDto>();
        CreateMap<WorkspaceRequestDto, Workspace>();

        // WorkspaceMember
        CreateMap<WorkspaceMember, WorkspaceMemberResponseDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.ClientEmail, opt => opt.MapFrom(src => src.Client.Email))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

        // Auth
        CreateMap<Client, LoginResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "CLIENT"));
        CreateMap<Admin, LoginResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "ADMIN"));

        // Role
        CreateMap<Role, RoleResponseDto>();

        // TimeEntry
        CreateMap<TimeEntry, TimeEntryResponseDto>();
        CreateMap<TimeEntryRequestDto, TimeEntry>();

        // Task — CompletedHours calculé depuis TimeEntries
        CreateMap<ProjectTask, TaskResponseDto>()
            .ForMember(dest => dest.CompletedHours,
                       opt => opt.MapFrom(src => src.TimeEntries.Sum(e => e.Hours)));
    }
}