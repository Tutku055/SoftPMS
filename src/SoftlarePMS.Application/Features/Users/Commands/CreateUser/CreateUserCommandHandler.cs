using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Exceptions;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.DTOs.User;
using SoftlarePMS.Domain.Entities;
using BCrypt.Net;

namespace SoftlarePMS.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.Users.AnyAsync(u => u.Username == request.Dto.Username, cancellationToken);
        if (exists)
            throw new Domain.Exceptions.DomainException($"Username '{request.Dto.Username}' is already taken.");

        exists = await context.Users.AnyAsync(u => u.Email == request.Dto.Email, cancellationToken);
        if (exists)
            throw new Domain.Exceptions.DomainException($"Email '{request.Dto.Email}' is already taken.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.Dto.EmployeeId,
            Username = request.Dto.Username,
            Email = request.Dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Dto.Password),
            IsActive = request.Dto.IsActive
        };

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<UserDto>(user);
    }
}
