using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Application.DTOs.Department;
using MediatR;

namespace SoftlarePMS.Application.Features.Departments.Queries.GetDepartmentsWithPagination;

public sealed record GetDepartmentsWithPaginationQuery : IRequest<PaginatedList<DepartmentDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
    public List<FilterCriteria>? Filters { get; init; }
}
