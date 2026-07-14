using AutoMapper;
using SoftlarePMS.Application.DTOs.User;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations;

public class UserService : BaseService<User, UserDto, CreateUserDto, UpdateUserDto>, IUserService
{
    public UserService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork.Users, unitOfWork, mapper)
    {
    }

    public override async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var exists = await _unitOfWork.Users.GetByUsernameAsync(dto.Username);
        if (exists != null)
            throw new ValidationException($"Username '{dto.Username}' is already taken.");

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = "hashed_password"; // Needs proper hashing service integration

        await _repository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }
}
