using MediatR;
using SoftPMS.Application.DTOs.Permission;

namespace SoftPMS.Application.Features.Permissions.Commands.UpdatePermission;

public record UpdatePermissionCommand(Guid Id, UpdatePermissionDto Dto) : IRequest<Unit>;
