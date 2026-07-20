using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Application.DTOs.User;

namespace SoftPMS.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
