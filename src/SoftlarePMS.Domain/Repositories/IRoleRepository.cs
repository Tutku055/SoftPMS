using System;
using System.Collections.Generic;
using System.Text;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Domain.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);

        Task<Role?> GetByIdWithPermissionsAsync(Guid id);
    }
}
