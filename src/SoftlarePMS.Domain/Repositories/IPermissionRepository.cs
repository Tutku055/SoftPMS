using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Domain.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<Permission?> GetByNameAsync(string name);
}
