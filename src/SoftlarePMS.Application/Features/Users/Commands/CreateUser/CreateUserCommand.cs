using MediatR;
using SoftlarePMS.Application.DTOs.User;

namespace SoftlarePMS.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(CreateUserDto Dto) : IRequest<UserDto>;
