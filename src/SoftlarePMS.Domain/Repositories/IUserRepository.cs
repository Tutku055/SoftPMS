using System;
using System.Collections.Generic;
using System.Text;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);

        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByIdWithRolesAsync(Guid id);
    }
}
