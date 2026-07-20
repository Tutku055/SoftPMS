using MediatR;
using SoftPMS.Application.DTOs.User;

namespace SoftPMS.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, UpdateUserDto Dto) : IRequest<Unit>;
