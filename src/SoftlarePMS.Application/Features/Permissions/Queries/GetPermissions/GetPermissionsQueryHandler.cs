using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.DTOs.Permission;

namespace SoftlarePMS.Application.Features.Permissions.Queries.GetPermissions;

public sealed class GetPermissionsQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
{
    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await context.Permissions
            .AsNoTracking()
            .ProjectTo<PermissionDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
