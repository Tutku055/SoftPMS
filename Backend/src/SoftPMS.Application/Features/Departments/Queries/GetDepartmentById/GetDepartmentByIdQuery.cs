using MediatR;
using SoftPMS.Application.DTOs.Department;

namespace SoftPMS.Application.Features.Departments.Queries.GetDepartmentById;

public sealed record GetDepartmentByIdQuery(Guid Id) : IRequest<DepartmentDto>;
