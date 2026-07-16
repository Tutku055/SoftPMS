using MediatR;
using SoftlarePMS.Application.DTOs.Permission;

namespace SoftlarePMS.Application.Features.Permissions.Commands.UpdatePermission;

public record UpdatePermissionCommand(Guid Id, UpdatePermissionDto Dto) : IRequest<Unit>;
