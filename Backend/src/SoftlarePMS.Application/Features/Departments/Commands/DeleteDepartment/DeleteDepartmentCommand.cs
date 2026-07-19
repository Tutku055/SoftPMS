using MediatR;

namespace SoftlarePMS.Application.Features.Departments.Commands.DeleteDepartment;

public sealed record DeleteDepartmentCommand(Guid Id) : IRequest<Unit>;
