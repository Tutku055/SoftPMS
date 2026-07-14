using AutoMapper;
using SoftlarePMS.Application.DTOs.Employee;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Exceptions;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations;

public class EmployeeService : BaseService<Employee, EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto>, IEmployeeService
{
    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork.Employees, unitOfWork, mapper)
    {
    }

    public async Task<EmployeeDetailDto> GetByIdWithDetailsAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetByIdWithDetailsAsync(id);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), id);

        return _mapper.Map<EmployeeDetailDto>(employee);
    }
}
