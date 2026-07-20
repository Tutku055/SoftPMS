using MediatR;
using SoftPMS.Application.DTOs.Role;

namespace SoftPMS.Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;
