using MediatR;

namespace SoftPMS.Application.Features.Roles.Commands.ToggleRoleActive;

public record ToggleRoleActiveCommand(Guid Id) : IRequest<Unit>;
