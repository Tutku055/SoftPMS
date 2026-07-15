using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Repositories;
using SoftlarePMS.Persistence.Context;

namespace SoftlarePMS.Persistence.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(SoftlarePMSDbContext context) : base(context)
    {
    }

    /// <summary>Looks up an employee by their business-key employee number.</summary>
    public async Task<Employee?> GetByEmployeeNoAsync(string employeeNo)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeNo == employeeNo);
    }

    /// <summary>
    /// Returns the employee with all related collections eagerly loaded via split queries
    /// (avoids cartesian explosion across five one-to-many relationships).
    /// </summary>
    public async Task<Employee?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.Employees
            .AsSplitQuery()
            .Include(e => e.Addresses)
            .Include(e => e.Compensations)
            .Include(e => e.Documents)
            .Include(e => e.Notes)
            .Include(e => e.References)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <summary>Returns the currently active address for the given employee (EndDate == null).</summary>
    public async Task<EmployeeAddress?> GetActiveAddressAsync(Guid employeeId)
    {
        return await _context.EmployeeAddresses
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.EndDate == null);
    }

    /// <summary>Returns the currently active compensation record (EndDate == null).</summary>
    public async Task<EmployeeCompensation?> GetActiveCompensationAsync(Guid employeeId)
    {
        return await _context.EmployeeCompensations
            .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.EndDate == null);
    }

    /// <summary>
    /// Returns all compensation records for an employee ordered newest-first.
    /// Used by rate history charts and the GetEmployeeRateHistoryQuery.
    /// </summary>
    public async Task<IEnumerable<EmployeeCompensation>> GetCompensationHistoryAsync(Guid employeeId)
    {
        return await _context.EmployeeCompensations
            .Where(c => c.EmployeeId == employeeId)
            .OrderByDescending(c => c.EffectiveDate)
            .ToListAsync();
    }
}
