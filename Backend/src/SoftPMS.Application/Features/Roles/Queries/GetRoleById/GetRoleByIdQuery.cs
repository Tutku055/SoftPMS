using MediatR;
using SoftPMS.Application.DTOs.Role;

namespace SoftPMS.Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto>;
