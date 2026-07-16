using MediatR;

namespace SoftlarePMS.Application.Features.Permissions.Commands.DeletePermission;

public record DeletePermissionCommand(Guid Id) : IRequest<Unit>;
