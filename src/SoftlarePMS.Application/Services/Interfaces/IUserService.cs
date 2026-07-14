using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.User;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Services.Interfaces;

public interface IUserService : IBaseService<User, UserDto, CreateUserDto, UpdateUserDto>
{
}
