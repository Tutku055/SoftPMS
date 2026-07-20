using MediatR;
using SoftPMS.Application.DTOs.Role;

namespace SoftPMS.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(CreateRoleDto Dto) : IRequest<RoleDto>;
