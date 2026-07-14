using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Employee;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return employees.Select(e => new EmployeeDto(
            e.Id,
            e.EmployeeNo,
            e.FirstName,
            e.LastName,
            e.Gender,
            e.EmploymentStatus,
            e.Profession,
            e.HireDate,
            e.TerminationDate,
            e.ProbationEndDate,
            e.WorkingHoursPerWeek,
            e.VacationDaysTotal
        ));
    }

    public async Task<EmployeeDetailDto> GetByIdAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetByIdWithDetailsAsync(id);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), id);

        return new EmployeeDetailDto(
            employee.Id,
            employee.EmployeeNo,
            employee.FirstName,
            employee.LastName,
            employee.Gender,
            employee.DateOfBirth,
            employee.Nationality,
            employee.Profession,
            employee.EmploymentStatus,
            employee.HireDate,
            employee.TerminationDate,
            employee.ProbationEndDate,
            employee.WorkingHoursPerWeek,
            employee.VacationDaysTotal,
            new List<SoftlarePMS.Application.DTOs.EmployeeAddress.EmployeeAddressDto>(), // Simplified mapping
            new List<SoftlarePMS.Application.DTOs.EmployeeCompensation.EmployeeCompensationDto>(),
            new List<SoftlarePMS.Application.DTOs.EmployeeDocument.EmployeeDocumentDto>(),
            new List<SoftlarePMS.Application.DTOs.EmployeeNote.EmployeeNoteDto>(),
            new List<SoftlarePMS.Application.DTOs.EmployeeReference.EmployeeReferenceDto>()
        );
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            EmployeeNo = dto.EmployeeNo,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            Nationality = dto.Nationality,
            Profession = dto.Profession,
            EmploymentStatus = dto.EmploymentStatus,
            HireDate = dto.HireDate,
            WorkingHoursPerWeek = dto.WorkingHoursPerWeek,
            VacationDaysTotal = dto.VacationDaysTotal,
            CreatedByUserId = 1 // Assuming default or requires context
        };

        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        return new EmployeeDto(
            employee.Id,
            employee.EmployeeNo,
            employee.FirstName,
            employee.LastName,
            employee.Gender,
            employee.EmploymentStatus,
            employee.Profession,
            employee.HireDate,
            employee.TerminationDate,
            employee.ProbationEndDate,
            employee.WorkingHoursPerWeek,
            employee.VacationDaysTotal
        );
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), id);

        _unitOfWork.Employees.Delete(employee);
        await _unitOfWork.SaveChangesAsync();
    }
}
