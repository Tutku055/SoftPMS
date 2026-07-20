using MediatR;
using SoftPMS.Application.DTOs.Department;

namespace SoftPMS.Application.Features.Departments.Commands.CreateDepartment;

public sealed record CreateDepartmentCommand(
    string Name,
    string Description
) : IRequest<DepartmentDto>;
