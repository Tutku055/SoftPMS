using MediatR;
using SoftPMS.Application.DTOs.Permission;

namespace SoftPMS.Application.Features.Permissions.Commands.CreatePermission;

public record CreatePermissionCommand(CreatePermissionDto Dto) : IRequest<PermissionDto>;
