using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Domain.Entities;

namespace SoftlarePMS.Persistence.Context;

public class SoftlarePMSDbContext : DbContext
{
    public SoftlarePMSDbContext(DbContextOptions<SoftlarePMSDbContext> options) : base(options)
    {
    }

    #region Security & User Management DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    #endregion

    #region Core Personnel DbSets
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeAddress> EmployeeAddresses { get; set; }
    public DbSet<EmployeeCompensation> EmployeeCompensations { get; set; }
    public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
    public DbSet<EmployeeNote> EmployeeNotes { get; set; }
    public DbSet<EmployeeReference> EmployeeReferences { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Will automatically apply all configurations from the assembly where SoftlarePMSDbContext is defined.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SoftlarePMSDbContext).Assembly);
    }

    // Override SaveChangesAsync to automatically set CreatedAt for new entities
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && e.State == EntityState.Added);

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
