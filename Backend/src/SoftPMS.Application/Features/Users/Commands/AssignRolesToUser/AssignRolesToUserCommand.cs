using MediatR;

namespace SoftPMS.Application.Features.Users.Commands.AssignRolesToUser;

/// <summary>Replaces the full set of roles for the given user (idempotent).</summary>
public sealed record AssignRolesToUserCommand(
    Guid UserId,
    List<Guid> RoleIds
) : IRequest<Unit>;
