using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Employee;

namespace SoftlarePMS.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDetailDto> GetByIdAsync(Guid id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task DeleteAsync(Guid id);
}
