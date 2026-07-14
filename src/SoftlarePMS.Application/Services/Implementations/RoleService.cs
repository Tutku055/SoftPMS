using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Role;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();
        return roles.Select(r => new RoleDto(r.Id, r.Name, r.Description));
    }

    public async Task<RoleDto> GetByIdAsync(Guid id)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role == null)
            throw new NotFoundException(nameof(Role), id);

        return new RoleDto(role.Id, role.Name, role.Description);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        var exists = await _unitOfWork.Roles.GetByNameAsync(dto.Name);
        if (exists != null)
            throw new ValidationException($"Role '{dto.Name}' already exists.");

        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _unitOfWork.Roles.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return new RoleDto(role.Id, role.Name, role.Description);
    }

    public async Task DeleteAsync(Guid id)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role == null)
            throw new NotFoundException(nameof(Role), id);

        _unitOfWork.Roles.Delete(role);
        await _unitOfWork.SaveChangesAsync();
    }
}
