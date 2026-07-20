using MediatR;
using SoftPMS.Application.Common.Models;
using SoftPMS.Application.DTOs.Employee;

namespace SoftPMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;

public sealed record GetEmployeesWithPaginationQuery(
    int PageNumber,
    int PageSize,
    List<FilterCriteria>? Filters
) : IRequest<PaginatedList<EmployeeDto>>;