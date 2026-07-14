using AutoMapper;
using SoftlarePMS.Application.DTOs.Role;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations;

public class RoleService : BaseService<Role, RoleDto, CreateRoleDto, UpdateRoleDto>, IRoleService
{
    public RoleService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork.Roles, unitOfWork, mapper)
    {
    }

    public override async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        var exists = await _unitOfWork.Roles.GetByNameAsync(dto.Name);
        if (exists != null)
            throw new ValidationException($"Role '{dto.Name}' already exists.");

        return await base.CreateAsync(dto);
    }
}
