using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Repositories;
using SoftlarePMS.Persistence.Context;

namespace SoftlarePMS.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    // Constructor
    public UserRepository(SoftlarePMSDbContext context) : base(context)
    {
    }

    //Get user by username
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    //Get user by email
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    //Get user by id with roles
    public async Task<User?> GetByIdWithRolesAsync(Guid id)
    {
        // Many-to-Many ilişkisini UserRole üzerinden çekiyoruz
        return await _context.Users
            .Include(u => u.Email) // Opsiyonel diğer ilişkileriniz
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
