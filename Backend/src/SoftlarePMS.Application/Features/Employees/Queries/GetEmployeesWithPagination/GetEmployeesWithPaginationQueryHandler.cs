using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Extensions;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Application.DTOs.Employee;

namespace SoftlarePMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;

public sealed class GetEmployeesWithPaginationQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetEmployeesWithPaginationQuery, PaginatedList<EmployeeDto>>
{
    public async Task<PaginatedList<EmployeeDto>> Handle(
        GetEmployeesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Employees
            .Include(e => e.Department)
            .AsNoTracking();

        // Tek satırda tüm dinamik filtreleri uygula
        query = query.ApplyDynamicFilters(request.Filters);

        query = query.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);

        var projectedQuery = query.ProjectTo<EmployeeDto>(mapper.ConfigurationProvider);

        return await PaginatedList<EmployeeDto>.CreateAsync(
            projectedQuery,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}