using MediatR;
using SoftlarePMS.Application.DTOs.Role;

namespace SoftlarePMS.Application.Features.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(Guid Id, UpdateRoleDto Dto) : IRequest<Unit>;
