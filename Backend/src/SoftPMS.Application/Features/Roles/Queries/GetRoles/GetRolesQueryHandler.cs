using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Application.DTOs.Role;

namespace SoftPMS.Application.Features.Roles.Queries.GetRoles;

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
