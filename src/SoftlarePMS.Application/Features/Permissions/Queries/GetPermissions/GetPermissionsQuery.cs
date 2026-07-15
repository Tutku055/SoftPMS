using MediatR;
using SoftlarePMS.Application.DTOs.Permission;

namespace SoftlarePMS.Application.Features.Permissions.Queries.GetPermissions;

public record GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>;
