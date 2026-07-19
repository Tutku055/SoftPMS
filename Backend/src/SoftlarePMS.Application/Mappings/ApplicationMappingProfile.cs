using AutoMapper;
using SoftlarePMS.Application.DTOs.Department;
using SoftlarePMS.Application.DTOs.Employee;
using SoftlarePMS.Application.DTOs.EmployeeAddress;
using SoftlarePMS.Application.DTOs.EmployeeCompensation;
using SoftlarePMS.Application.DTOs.EmployeeDocument;
using SoftlarePMS.Application.DTOs.EmployeeNote;
using SoftlarePMS.Application.DTOs.EmployeeReference;
using SoftlarePMS.Application.DTOs.Permission;
using SoftlarePMS.Application.DTOs.Role;
using SoftlarePMS.Application.DTOs.User;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Mappings;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        // Employee mappings
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, EmployeeDetailDto>();
        // Downcast for facade: detail -> slim DTO
        CreateMap<EmployeeDetailDto, EmployeeDto>();
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());

        // Employee address mappings
        CreateMap<EmployeeAddress, EmployeeAddressDto>();
        CreateMap<CreateEmployeeAddressDto, EmployeeAddress>()
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.IsPrimary, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore());
        CreateMap<UpdateEmployeeAddressDto, EmployeeAddress>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.IsPrimary, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore());

        // Department mappings
        CreateMap<Department, DepartmentDto>().ReverseMap();

        // Employee compensation mappings
        CreateMap<EmployeeCompensation, EmployeeCompensationDto>()
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.EndDate == null));
        CreateMap<CreateEmployeeCompensationDto, EmployeeCompensation>()
            .ForMember(d => d.EndDate, o => o.Ignore())
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.CreatedByUserId, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore())
            .ForMember(d => d.CreatedByUser, o => o.Ignore());
        CreateMap<UpdateEmployeeCompensationDto, EmployeeCompensation>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.CreatedByUserId, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore())
            .ForMember(d => d.CreatedByUser, o => o.Ignore());

        // Employee document mappings
        CreateMap<EmployeeDocument, EmployeeDocumentDto>()
            .ForMember(d => d.UploadedAt, o => o.MapFrom(s => s.CreatedAt));
        CreateMap<CreateEmployeeDocumentDto, EmployeeDocument>()
            .ForMember(d => d.IssueDate, o => o.Ignore())
            .ForMember(d => d.ExpiryDate, o => o.Ignore())
            .ForMember(d => d.ReminderDate, o => o.Ignore())
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.CreatedByUserId, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore())
            .ForMember(d => d.CreatedByUser, o => o.Ignore());
        CreateMap<UpdateEmployeeDocumentDto, EmployeeDocument>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.CreatedByUserId, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore())
            .ForMember(d => d.CreatedByUser, o => o.Ignore());

        // Employee note mappings
        CreateMap<EmployeeNote, EmployeeNoteDto>();
        CreateMap<CreateEmployeeNoteDto, EmployeeNote>()
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.CreatedByUserId, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore())
            .ForMember(d => d.CreatedByUser, o => o.Ignore());
        CreateMap<UpdateEmployeeNoteDto, EmployeeNote>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.CreatedByUserId, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore())
            .ForMember(d => d.CreatedByUser, o => o.Ignore());

        // Employee reference mappings
        CreateMap<EmployeeReference, EmployeeReferenceDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.ContactPerson))
            .ForMember(d => d.Company, o => o.MapFrom(s => s.CompanyName))
            .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Phone));
        CreateMap<CreateEmployeeReferenceDto, EmployeeReference>()
            .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company))
            .ForMember(d => d.ContactPerson, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.PhoneNumber))
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore());
        CreateMap<UpdateEmployeeReferenceDto, EmployeeReference>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company))
            .ForMember(d => d.ContactPerson, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.PhoneNumber))
            .ForMember(d => d.EmployeeId, o => o.Ignore())
            .ForMember(d => d.Employee, o => o.Ignore());

        // Role mappings
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());

        // Permission mappings
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<CreatePermissionDto, Permission>();
        CreateMap<UpdatePermissionDto, Permission>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());

        // User mappings
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());
    }
}