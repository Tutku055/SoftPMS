using MediatR;
using SoftPMS.Application.Common.Models;
using SoftPMS.Application.DTOs.User;

namespace SoftPMS.Application.Features.Users.Queries.GetUsersWithPagination;

public sealed record GetUsersWithPaginationQuery(
    int PageNumber,
    int PageSize,
    List<FilterCriteria>? Filters
) : IRequest<PaginatedList<UserDto>>;
