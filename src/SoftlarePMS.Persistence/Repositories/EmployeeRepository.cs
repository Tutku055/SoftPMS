using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Repositories;
using SoftlarePMS.Persistence.Context;

namespace SoftlarePMS.Persistence.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    // Constructor
    public EmployeeRepository(SoftlarePMSDbContext context) : base(context)
    {
    }

    // Method to get an employee by their employee number
    public async Task<Employee?> GetByEmployeeNoAsync(string employeeNo)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeNo == employeeNo);
    }

    // Method to get an employee by their ID with related details
    public async Task<Employee?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.Employees
            .Include(e => e.Addresses)
            .Include(e => e.Compensations)
            .Include(e => e.Documents)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
