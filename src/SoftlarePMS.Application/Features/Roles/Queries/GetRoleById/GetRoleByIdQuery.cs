using MediatR;
using SoftlarePMS.Application.DTOs.Role;

namespace SoftlarePMS.Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto>;
