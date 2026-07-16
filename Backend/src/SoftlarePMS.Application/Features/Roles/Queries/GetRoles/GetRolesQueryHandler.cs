using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.DTOs.Role;

namespace SoftlarePMS.Application.Features.Roles.Queries.GetRoles;

public sealed class GetRolesQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
    public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await context.Roles
            .AsNoTracking()
            .ProjectTo<RoleDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
