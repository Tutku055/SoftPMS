using MediatR;
using SoftlarePMS.Application.DTOs.Permission;

namespace SoftlarePMS.Application.Features.Permissions.Queries.GetPermissionById;

public record GetPermissionByIdQuery(Guid Id) : IRequest<PermissionDto>;
