using AutoMapper;
using MediatR;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.DTOs.Department;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Features.Departments.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandHandler(
    IApplicationDbContext context,
    IMapper mapper,
    IDateTime dateTime)
    : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = dateTime.UtcNow
        };

        await context.Departments.AddAsync(department, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<DepartmentDto>(department);
    }
}
