using MediatR;

namespace SoftPMS.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<Unit>;
