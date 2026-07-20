using MediatR;

namespace SoftPMS.Application.Features.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid Id) : IRequest<Unit>;
