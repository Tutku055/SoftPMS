using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Permission;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Services.Interfaces;

public interface IPermissionService : IBaseService<Permission, PermissionDto, CreatePermissionDto, UpdatePermissionDto>
{
}
