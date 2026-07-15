using MediatR;
using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Application.DTOs.Employee;
using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;

/// <summary>
/// Server-side filtered, paginated employee list query.
/// All filter parameters are optional (null = no filter applied for that field).
/// </summary>
public sealed record GetEmployeesWithPaginationQuery(
    int PageNumber,
    int PageSize,

    // --- Filter parameters ---
    string? City,
    string? Profession,
    EmploymentStatus? EmploymentStatus,
    decimal? MinRate,
    decimal? MaxRate
) : IRequest<PaginatedList<EmployeeDto>>;
