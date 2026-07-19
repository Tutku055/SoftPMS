using MediatR;
using SoftlarePMS.Application.DTOs.Department;

namespace SoftlarePMS.Application.Features.Departments.Queries.GetDepartmentsLookup;

public sealed record GetDepartmentsLookupQuery : IRequest<List<DepartmentLookupDto>>;
