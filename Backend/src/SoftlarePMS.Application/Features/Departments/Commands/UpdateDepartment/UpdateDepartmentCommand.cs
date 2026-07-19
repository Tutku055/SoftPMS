using MediatR;

namespace SoftlarePMS.Application.Features.Departments.Commands.UpdateDepartment;

public sealed record UpdateDepartmentCommand(
    Guid Id,
    string Name,
    string Description
) : IRequest<Unit>;
