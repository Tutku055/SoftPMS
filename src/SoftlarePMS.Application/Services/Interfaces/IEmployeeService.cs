using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftlarePMS.Application.DTOs.Employee;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Services.Interfaces;

public interface IEmployeeService : IBaseService<Employee, EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto>
{
    Task<EmployeeDetailDto> GetByIdWithDetailsAsync(Guid id);
}
