using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.User;

namespace SoftlarePMS.Application.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(Guid id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task DeleteAsync(Guid id);
}
