using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Role;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Services.Interfaces;

public interface IRoleService : IBaseService<Role, RoleDto, CreateRoleDto, UpdateRoleDto>
{
}
