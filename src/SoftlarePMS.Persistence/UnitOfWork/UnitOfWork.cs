using SoftlarePMS.Domain.Repositories;
using SoftlarePMS.Domain.UnitOfWork;
using SoftlarePMS.Persistence.Context;

namespace SoftlarePMS.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SoftlarePMSDbContext _context;

    public IUserRepository Users { get; private set; }
    public IRoleRepository Roles { get; private set; }
    public IPermissionRepository Permissions { get; private set; }
    public IEmployeeRepository Employees { get; private set; }

    public UnitOfWork(SoftlarePMSDbContext context, 
                      IUserRepository userRepository, 
                      IRoleRepository roleRepository, 
                      IPermissionRepository permissionRepository, 
                      IEmployeeRepository employeeRepository)
    {
        _context = context;
        Users = userRepository;
        Roles = roleRepository;
        Permissions = permissionRepository;
        Employees = employeeRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
