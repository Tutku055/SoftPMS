using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Permission;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations;

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PermissionDto>> GetAllAsync()
    {
        var permissions = await _unitOfWork.Permissions.GetAllAsync();
        return permissions.Select(p => new PermissionDto(p.Id, p.Name, p.Description));
    }

    public async Task<PermissionDto> GetByIdAsync(Guid id)
    {
        var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
        if (permission == null)
            throw new NotFoundException(nameof(Permission), id);

        return new PermissionDto(permission.Id, permission.Name, permission.Description);
    }

    public async Task<PermissionDto> CreateAsync(CreatePermissionDto dto)
    {
        var exists = await _unitOfWork.Permissions.GetByNameAsync(dto.Name);
        if (exists != null)
            throw new ValidationException($"Permission '{dto.Name}' already exists.");

        var permission = new Permission
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _unitOfWork.Permissions.AddAsync(permission);
        await _unitOfWork.SaveChangesAsync();

        return new PermissionDto(permission.Id, permission.Name, permission.Description);
    }

    public async Task DeleteAsync(Guid id)
    {
        var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
        if (permission == null)
            throw new NotFoundException(nameof(Permission), id);

        _unitOfWork.Permissions.Delete(permission);
        await _unitOfWork.SaveChangesAsync();
    }
}
