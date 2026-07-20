using MediatR;
using SoftPMS.Application.DTOs.Permission;

namespace SoftPMS.Application.Features.Permissions.Queries.GetPermissions;

public record GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>;
