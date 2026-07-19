using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.DTOs.Department;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Departments.Queries.GetDepartmentById;

public sealed class GetDepartmentByIdQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await context.Departments
            .AsNoTracking()
            .Where(d => d.Id == request.Id)
            .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (department is null)
        {
            throw new NotFoundException(nameof(Department), request.Id);
        }

        return department;
    }
}
