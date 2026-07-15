using MediatR;
using SoftlarePMS.Application.DTOs.Permission;

namespace SoftlarePMS.Application.Features.Permissions.Commands.CreatePermission;

public record CreatePermissionCommand(CreatePermissionDto Dto) : IRequest<PermissionDto>;
