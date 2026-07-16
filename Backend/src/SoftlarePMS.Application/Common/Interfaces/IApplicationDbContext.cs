using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Application.Common.Interfaces;

/// <summary>
/// EF Core DbContext abstraction exposed to the Application layer.
/// Handlers use this interface instead of the concrete DbContext to keep
/// the Application layer independent of Persistence infrastructure.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; }
    DbSet<EmployeeAddress> EmployeeAddresses { get; }
    DbSet<EmployeeCompensation> EmployeeCompensations { get; }
    DbSet<EmployeeDocument> EmployeeDocuments { get; }
    DbSet<EmployeeNote> EmployeeNotes { get; }
    DbSet<EmployeeReference> EmployeeReferences { get; }
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
