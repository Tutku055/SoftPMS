using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Repositories;
using SoftlarePMS.Persistence.Context;

namespace SoftlarePMS.Persistence.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    //Constructor
    public RoleRepository(SoftlarePMSDbContext context) : base(context)
    {
    }

    //Get role by name
    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    //Get role by id with permissions
    public async Task<Role?> GetByIdWithPermissionsAsync(Guid id)
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
