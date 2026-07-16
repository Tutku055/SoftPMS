using MediatR;

namespace SoftlarePMS.Application.Features.Roles.Commands.AssignPermissionsToRole;

/// <summary>Replaces the full set of permissions for the given role (idempotent).</summary>
public sealed record AssignPermissionsToRoleCommand(
    Guid RoleId,
    List<Guid> PermissionIds
) : IRequest<Unit>;
