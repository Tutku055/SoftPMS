using MediatR;
using SoftlarePMS.Application.DTOs.Department;

namespace SoftlarePMS.Application.Features.Departments.Commands.CreateDepartment;

public sealed record CreateDepartmentCommand(
    string Name,
    string Description
) : IRequest<DepartmentDto>;
