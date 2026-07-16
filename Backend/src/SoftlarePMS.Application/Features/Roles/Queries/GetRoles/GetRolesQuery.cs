using MediatR;
using SoftlarePMS.Application.DTOs.Role;

namespace SoftlarePMS.Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;
