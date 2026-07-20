using MediatR;
using SoftPMS.Application.DTOs.Role;

namespace SoftPMS.Application.Features.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(Guid Id, UpdateRoleDto Dto) : IRequest<Unit>;
