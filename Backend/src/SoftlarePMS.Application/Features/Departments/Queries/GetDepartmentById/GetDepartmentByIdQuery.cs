using MediatR;
using SoftlarePMS.Application.DTOs.Department;

namespace SoftlarePMS.Application.Features.Departments.Queries.GetDepartmentById;

public sealed record GetDepartmentByIdQuery(Guid Id) : IRequest<DepartmentDto>;
