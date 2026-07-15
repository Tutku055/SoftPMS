using MediatR;

namespace SoftlarePMS.Application.Features.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid Id) : IRequest<Unit>;
