using SoftlarePMS.Domain.Repositories;

namespace SoftlarePMS.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IPermissionRepository Permissions { get; }
    IEmployeeRepository Employees { get; }
    Task<int> SaveChangesAsync();
}
