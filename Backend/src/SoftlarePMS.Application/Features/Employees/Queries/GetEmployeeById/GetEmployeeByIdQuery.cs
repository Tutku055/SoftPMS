using MediatR;
using SoftlarePMS.Application.DTOs.Employee;

namespace SoftlarePMS.Application.Features.Employees.Queries.GetEmployeeById;

/// <summary>Query to retrieve a single employee with all related details (addresses, compensations, documents, notes, references).</summary>
public sealed record GetEmployeeByIdQuery(Guid EmployeeId) : IRequest<EmployeeDetailDto>;
