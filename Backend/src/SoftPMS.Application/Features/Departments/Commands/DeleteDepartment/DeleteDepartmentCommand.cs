using MediatR;

namespace SoftPMS.Application.Features.Departments.Commands.DeleteDepartment;

public sealed record DeleteDepartmentCommand(Guid Id) : IRequest<Unit>;
