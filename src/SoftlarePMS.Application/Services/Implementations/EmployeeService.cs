using AutoMapper;
using MediatR;
using SoftlarePMS.Application.DTOs.Employee;
using SoftlarePMS.Application.Features.Employees.Commands.CreateEmployee;
using SoftlarePMS.Application.Features.Employees.Commands.DeleteEmployee;
using SoftlarePMS.Application.Features.Employees.Commands.UpdateEmployee;
using SoftlarePMS.Application.Features.Employees.Queries.GetEmployeeById;
using SoftlarePMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.Services.Implementations;

/// <summary>
/// Facade implementation of IEmployeeService.
/// All business logic lives in MediatR handlers; this class is a thin forwarding layer
/// that bridges the legacy IBaseService contract with the CQRS pipeline.
/// </summary>
public class EmployeeService(IMediator mediator, IMapper mapper) : IEmployeeService
{
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        // Delegate to the paginated query; retrieve the first 1000 records for a "get all" scenario.
        var result = await mediator.Send(new GetEmployeesWithPaginationQuery(
            PageNumber: 1,
            PageSize: 1000,
            City: null,
            Profession: null,
            EmploymentStatus: null,
            MinRate: null,
            MaxRate: null));

        return result.Items;
    }

    public async Task<EmployeeDto> GetByIdAsync(Guid id)
    {
        // GetEmployeeByIdQuery returns EmployeeDetailDto; map down to the slim EmployeeDto.
        var detail = await mediator.Send(new GetEmployeeByIdQuery(id));
        return mapper.Map<EmployeeDto>(detail);
    }

    public async Task<EmployeeDetailDto> GetByIdWithDetailsAsync(Guid id)
    {
        return await mediator.Send(new GetEmployeeByIdQuery(id));
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        // Map the legacy DTO to the CQRS command. Address/compensation defaults are applied here.
        var command = new CreateEmployeeCommand(
            EmployeeNo:         dto.EmployeeNo,
            FirstName:          dto.FirstName,
            LastName:           dto.LastName,
            Gender:             dto.Gender,
            DateOfBirth:        dto.DateOfBirth,
            Nationality:        dto.Nationality,
            Profession:         dto.Profession,
            EmploymentStatus:   dto.EmploymentStatus,
            HireDate:           dto.HireDate,
            WorkingHoursPerWeek: dto.WorkingHoursPerWeek,
            VacationDaysTotal:  dto.VacationDaysTotal,
            // Use first provided address or sensible defaults
            AddressLine:        dto.Addresses.FirstOrDefault()?.AddressTitle ?? string.Empty,
            PostalCode:         dto.Addresses.FirstOrDefault()?.ZipCode ?? string.Empty,
            City:               dto.Addresses.FirstOrDefault()?.City ?? string.Empty,
            State:              dto.Addresses.FirstOrDefault()?.District ?? string.Empty,
            Country:            dto.Addresses.FirstOrDefault()?.Country ?? string.Empty,
            BaseSalary:         dto.Compensations.FirstOrDefault()?.BaseSalary ?? 0m,
            SalaryType:         dto.Compensations.FirstOrDefault()?.SalaryType ?? SalaryType.Monthly,
            PayGrade:           dto.Compensations.FirstOrDefault()?.PayGrade ?? string.Empty
        );

        var created = await mediator.Send(command);

        // Return slim EmployeeDto using created result
        return new EmployeeDto(
            Id:                 created.Id,
            EmployeeNo:         created.EmployeeNo,
            FirstName:          dto.FirstName,
            LastName:           dto.LastName,
            Gender:             dto.Gender,
            EmploymentStatus:   dto.EmploymentStatus,
            Profession:         dto.Profession,
            HireDate:           created.HireDate,
            TerminationDate:    null,
            ProbationEndDate:   null,
            WorkingHoursPerWeek: dto.WorkingHoursPerWeek,
            VacationDaysTotal:  dto.VacationDaysTotal
        );
    }

    public async Task UpdateAsync(Guid id, UpdateEmployeeDto dto)
    {
        await mediator.Send(new UpdateEmployeeCommand(
            EmployeeId:         id,
            FirstName:          dto.FirstName,
            LastName:           dto.LastName,
            Gender:             dto.Gender,
            DateOfBirth:        dto.DateOfBirth,
            Nationality:        dto.Nationality,
            Profession:         dto.Profession,
            EmploymentStatus:   dto.EmploymentStatus,
            HireDate:           dto.HireDate,
            TerminationDate:    null,
            ProbationEndDate:   null,
            WorkingHoursPerWeek: dto.WorkingHoursPerWeek,
            VacationDaysTotal:  dto.VacationDaysTotal
        ));
    }

    public async Task DeleteAsync(Guid id)
    {
        await mediator.Send(new DeleteEmployeeCommand(id));
    }
}
