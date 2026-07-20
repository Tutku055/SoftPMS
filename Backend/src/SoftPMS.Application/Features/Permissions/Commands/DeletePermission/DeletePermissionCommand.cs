using MediatR;

namespace SoftPMS.Application.Features.Permissions.Commands.DeletePermission;

public record DeletePermissionCommand(Guid Id) : IRequest<Unit>;
