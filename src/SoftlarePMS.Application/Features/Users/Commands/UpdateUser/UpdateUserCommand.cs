using MediatR;
using SoftlarePMS.Application.DTOs.User;

namespace SoftlarePMS.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, UpdateUserDto Dto) : IRequest<Unit>;
