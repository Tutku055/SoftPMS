using MediatR;
using SoftlarePMS.Application.DTOs.Role;

namespace SoftlarePMS.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(CreateRoleDto Dto) : IRequest<RoleDto>;
