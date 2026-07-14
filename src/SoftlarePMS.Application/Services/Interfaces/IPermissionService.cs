using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Permission;

namespace SoftlarePMS.Application.Services.Interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionDto>> GetAllAsync();
    Task<PermissionDto> GetByIdAsync(Guid id);
    Task<PermissionDto> CreateAsync(CreatePermissionDto dto);
    Task DeleteAsync(Guid id);
}
