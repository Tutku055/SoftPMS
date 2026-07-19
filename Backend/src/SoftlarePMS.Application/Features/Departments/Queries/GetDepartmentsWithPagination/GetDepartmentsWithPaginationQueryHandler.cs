using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Application.DTOs.Department;

namespace SoftlarePMS.Application.Features.Departments.Queries.GetDepartmentsWithPagination;

public sealed class GetDepartmentsWithPaginationQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetDepartmentsWithPaginationQuery, PaginatedList<DepartmentDto>>
{
    public async Task<PaginatedList<DepartmentDto>> Handle(
        GetDepartmentsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Departments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim().ToLower();
            query = query.Where(d => d.Name.ToLower().Contains(search) || d.Description.ToLower().Contains(search));
        }

        var projectedQuery = query
            .OrderBy(d => d.Name)
            .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider);

        return await PaginatedList<DepartmentDto>.CreateAsync(
            projectedQuery,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
