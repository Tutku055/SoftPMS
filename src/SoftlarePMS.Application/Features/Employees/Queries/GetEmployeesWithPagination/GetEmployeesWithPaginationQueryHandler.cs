using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Application.DTOs.Employee;

namespace SoftlarePMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;

/// <summary>
/// High-performance server-side filtering and pagination.
/// Builds a composable LINQ predicate chain so only the requested filters generate SQL WHERE clauses.
/// All projections happen in the database via AutoMapper's ProjectTo — no entity hydration.
/// </summary>
public sealed class GetEmployeesWithPaginationQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetEmployeesWithPaginationQuery, PaginatedList<EmployeeDto>>
{
    public async Task<PaginatedList<EmployeeDto>> Handle(
        GetEmployeesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        // Start from the root with the global soft-delete filter already applied
        var query = context.Employees.AsQueryable();

        // --- Dynamic filter composition ---

        if (!string.IsNullOrWhiteSpace(request.Profession))
        {
            query = query.Where(e =>
                e.Profession.Contains(request.Profession));
        }

        if (request.EmploymentStatus.HasValue)
        {
            query = query.Where(e => e.EmploymentStatus == request.EmploymentStatus.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            // Filter by active address city (EndDate == null)
            query = query.Where(e =>
                e.Addresses.Any(a => a.EndDate == null && a.City.Contains(request.City)));
        }

        if (request.MinRate.HasValue)
        {
            // Filter against the active compensation (EndDate == null)
            query = query.Where(e =>
                e.Compensations.Any(c => c.EndDate == null && c.BaseSalary >= request.MinRate.Value));
        }

        if (request.MaxRate.HasValue)
        {
            query = query.Where(e =>
                e.Compensations.Any(c => c.EndDate == null && c.BaseSalary <= request.MaxRate.Value));
        }

        // Sort by last name then first name for consistent ordering
        query = query.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);

        // Project in-database to EmployeeDto via AutoMapper (no entity materialisation)
        var projectedQuery = query.ProjectTo<EmployeeDto>(mapper.ConfigurationProvider);

        return await PaginatedList<EmployeeDto>.CreateAsync(
            projectedQuery,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
