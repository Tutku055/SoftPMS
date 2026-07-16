using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Application.DTOs.Employee;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Employees.Queries.GetEmployeeById;

/// <summary>
/// Fetches a single employee with all related collections in one optimised query.
/// Uses split queries (separate SQL per Include) to avoid cartesian explosion across five collections.
/// </summary>
public sealed class GetEmployeeByIdQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailDto>
{
    public async Task<EmployeeDetailDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await context.Employees
            .Include(e => e.Addresses)
            .Include(e => e.Compensations)
            .Include(e => e.Documents)
            .Include(e => e.Notes)
            .Include(e => e.References)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Employee), request.EmployeeId);

        return mapper.Map<EmployeeDetailDto>(employee);
    }
}
