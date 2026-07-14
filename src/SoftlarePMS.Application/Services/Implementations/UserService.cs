using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.User;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(u => new UserDto(u.Id, u.InternalId, u.EmployeeId, u.Username, u.Email, u.IsActive));
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException(nameof(User), id);

        return new UserDto(user.Id, user.InternalId, user.EmployeeId, user.Username, user.Email, user.IsActive);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var exists = await _unitOfWork.Users.GetByUsernameAsync(dto.Username);
        if (exists != null)
            throw new ValidationException($"Username '{dto.Username}' is already taken.");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = "hashed_password", // Needs proper hashing service integration
            EmployeeId = dto.EmployeeId,
            IsActive = dto.IsActive
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new UserDto(user.Id, user.InternalId, user.EmployeeId, user.Username, user.Email, user.IsActive);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException(nameof(User), id);

        _unitOfWork.Users.Delete(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
