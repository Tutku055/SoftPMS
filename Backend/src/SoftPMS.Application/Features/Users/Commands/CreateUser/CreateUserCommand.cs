using MediatR;
using SoftPMS.Application.DTOs.User;

namespace SoftPMS.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(CreateUserDto Dto) : IRequest<UserDto>;
