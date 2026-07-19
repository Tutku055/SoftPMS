using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;

namespace SoftlarePMS.Application.Features.Employees.Commands.CreateEmployee;

/// <summary>
/// Creates a new employee together with their initial address and compensation record
/// in a single atomic save. Checks for duplicate employee numbers before inserting.
/// </summary>
public sealed class CreateEmployeeCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTime dateTime)
    : IRequestHandler<CreateEmployeeCommand, CreatedEmployeeDto>
{
    public async Task<CreatedEmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Prevent duplicate employee numbers
        var duplicate = await context.Employees
            .AnyAsync(e => e.EmployeeNo == request.EmployeeNo, cancellationToken);

        if (duplicate)
            throw new DomainException($"An employee with number '{request.EmployeeNo}' already exists.");

        var now = dateTime.UtcNow;
        var actorId = currentUser.UserId;

        // Build the employee aggregate root
        var employee = new Employee
        {
            EmployeeNo        = request.EmployeeNo,
            FirstName         = request.FirstName,
            LastName          = request.LastName,
            Gender            = request.Gender,
            DateOfBirth       = request.DateOfBirth,
            Nationality       = request.Nationality,
            Profession        = request.Profession,
            EmploymentStatus  = request.EmploymentStatus,
            HireDate          = request.HireDate,
            WorkingHoursPerWeek = request.WorkingHoursPerWeek,
            VacationDaysTotal = request.VacationDaysTotal,
            CreatedByUserId   = actorId,
            CreatedAt         = now,
            IsDeleted         = false
        };

        // Initial address
        var initialAddress = new EmployeeAddress
        {
            Employee    = employee,
            AddressLine = request.AddressLine,
            PostalCode  = request.PostalCode,
            City        = request.City,
            State       = request.State,
            Country     = request.Country,
            IsPrimary   = true,
            CreatedAt   = now
        };

        // Initial compensation — EffectiveDate defaults to HireDate; EndDate is null (active)
        var initialCompensation = new EmployeeCompensation
        {
            Employee          = employee,
            BaseSalary        = request.BaseSalary,
            SalaryType        = request.SalaryType,
            PayGrade          = request.PayGrade,
            EffectiveDate     = request.HireDate,
            EndDate           = null,
            CreatedByUserId   = actorId,
            CreatedAt         = now
        };

        await context.Employees.AddAsync(employee, cancellationToken);
        await context.EmployeeAddresses.AddAsync(initialAddress, cancellationToken);
        await context.EmployeeCompensations.AddAsync(initialCompensation, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return new CreatedEmployeeDto(employee.Id, employee.EmployeeNo, employee.HireDate);
    }
}
