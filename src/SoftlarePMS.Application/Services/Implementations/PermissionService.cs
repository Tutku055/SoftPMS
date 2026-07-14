using AutoMapper;
using SoftlarePMS.Application.DTOs.Permission;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations;

public class PermissionService : BaseService<Permission, PermissionDto, CreatePermissionDto, UpdatePermissionDto>, IPermissionService
{
    public PermissionService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork.Permissions, unitOfWork, mapper)
    {
    }

    public override async Task<PermissionDto> CreateAsync(CreatePermissionDto dto)
    {
        var exists = await _unitOfWork.Permissions.GetByNameAsync(dto.Name);
        if (exists != null)
            throw new ValidationException($"Permission '{dto.Name}' already exists.");

        return await base.CreateAsync(dto);
    }
}
