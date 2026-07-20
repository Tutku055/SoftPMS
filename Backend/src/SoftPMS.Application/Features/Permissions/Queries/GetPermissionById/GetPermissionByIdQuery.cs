using MediatR;
using SoftPMS.Application.DTOs.Permission;

namespace SoftPMS.Application.Features.Permissions.Queries.GetPermissionById;

public record GetPermissionByIdQuery(Guid Id) : IRequest<PermissionDto>;
