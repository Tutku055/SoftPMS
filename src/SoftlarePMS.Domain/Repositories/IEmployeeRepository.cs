using System;
using System.Collections.Generic;
using System.Text;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetByEmployeeNoAsync(string employeeNo);

        Task<Employee?> GetByIdWithDetailsAsync(Guid id);
    }
}
