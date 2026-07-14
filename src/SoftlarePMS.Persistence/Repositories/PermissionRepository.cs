using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Repositories;
using SoftlarePMS.Persistence.Context;

namespace SoftlarePMS.Persistence.Repositories;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(SoftlarePMSDbContext context) : base(context)
    {
    }

    public async Task<Permission?> GetByNameAsync(string name)
    {
        return await _context.Set<Permission>().FirstOrDefaultAsync(p => p.Name == name);
    }
}
