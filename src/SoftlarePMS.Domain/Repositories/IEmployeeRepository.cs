using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Domain.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByEmployeeNoAsync(string employeeNo);

    Task<Employee?> GetByIdWithDetailsAsync(Guid id);

    /// <summary>Returns the currently active address (EndDate == null) for the given employee.</summary>
    Task<EmployeeAddress?> GetActiveAddressAsync(Guid employeeId);

    /// <summary>Returns the currently active compensation record (EndDate == null).</summary>
    Task<EmployeeCompensation?> GetActiveCompensationAsync(Guid employeeId);

    /// <summary>Returns all compensation records ordered newest-first, for rate history charts.</summary>
    Task<IEnumerable<EmployeeCompensation>> GetCompensationHistoryAsync(Guid employeeId);
}
