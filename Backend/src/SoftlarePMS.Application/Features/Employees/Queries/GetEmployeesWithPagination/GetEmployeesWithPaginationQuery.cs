using MediatR;
using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Application.DTOs.Employee;

namespace SoftlarePMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;

public sealed record GetEmployeesWithPaginationQuery(
    int PageNumber,
    int PageSize,
    List<FilterCriteria>? Filters
) : IRequest<PaginatedList<EmployeeDto>>;