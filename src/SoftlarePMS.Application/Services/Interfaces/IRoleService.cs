using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Role;

namespace SoftlarePMS.Application.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto> GetByIdAsync(Guid id);
    Task<RoleDto> CreateAsync(CreateRoleDto dto);
    Task DeleteAsync(Guid id);
}
