using MediatR;
using SoftPMS.Application.DTOs.Department;

namespace SoftPMS.Application.Features.Departments.Queries.GetDepartmentsLookup;

public sealed record GetDepartmentsLookupQuery : IRequest<List<DepartmentLookupDto>>;
