using System;
using MediatR;

namespace SoftPMS.Application.Features.Users.Commands.AssignRoleToUser;

public sealed record AssignRoleToUserCommand(Guid UserId, Guid RoleId) : IRequest<Unit>;
