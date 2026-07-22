using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoftPMS.Domain.Entities;
using SoftPMS.Persistence.Context;

namespace SoftPMS.Persistence;

/// <summary>
/// Runs at application startup to ensure the database schema is up to date
/// and the seed data (permissions, admin role, admin user) is present.
/// Safe to re-run — all inserts are conditional on existing data.
/// </summary>
public static class DatabaseSeeder
{
    // All system-defined permissions in "Resource.Action" format
    private static readonly (string Name, string Description)[] SeedPermissions =
    [
        ("Employees.Read",   "View employee records"),
        ("Employees.Create", "Create new employee records"),
        ("Employees.Update", "Edit existing employee records"),
        ("Employees.Delete", "Delete employee records"),
        ("Departments.Read", "View departments"),
        ("Departments.Create", "Create new departments"),
        ("Departments.Update", "Edit departments"),
        ("Departments.Delete", "Delete departments"),
        ("Documents.Read",   "View documents"),
        ("Documents.Create", "Upload documents"),
        ("Documents.Update", "Edit documents"),
        ("Documents.Delete", "Delete documents"),
        ("Roles.Read",       "View roles"),
        ("Roles.Create",     "Create new roles"),
        ("Roles.Update",     "Edit roles"),
        ("Roles.Delete",     "Delete roles"),
        ("Users.Read",       "View user accounts"),
        ("Users.Create",     "Create user accounts"),
        ("Users.Update",     "Edit user accounts"),
        ("Users.Delete",     "Delete user accounts"),
        ("Permissions.Read", "View available permissions"),
    ];

    private const string AdminRoleName = "Admin";
    private const string AdminUsername  = "admin";
    private const string AdminEmail     = "admin@SoftPMS.com";
    private const string AdminPassword  = "Admin@123!"; // Override via env / user-secrets in production

    public static async Task SeedAsync(IServiceProvider rootProvider, CancellationToken ct = default)
    {
        await using var scope = rootProvider.CreateAsyncScope();
        var sp     = scope.ServiceProvider;
        var db     = sp.GetRequiredService<SoftPMSDbContext>();
        var logger = sp.GetRequiredService<ILogger<SoftPMSDbContext>>();

        try
        {
            // ── 1. Apply pending EF Core migrations ──────────────────────────
            await db.Database.MigrateAsync(ct);
            logger.LogInformation("Database migrations applied successfully.");

            // ── 2. Seed permissions ──────────────────────────────────────────
            var existingNames = await db.Permissions
                .Select(p => p.Name)
                .ToHashSetAsync(ct);

            var missing = SeedPermissions
                .Where(x => !existingNames.Contains(x.Name))
                .Select(x => new Permission { Name = x.Name, Description = x.Description })
                .ToList();

            if (missing.Count > 0)
            {
                db.Permissions.AddRange(missing);
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Seeded {Count} new permission(s).", missing.Count);
            }

            // ── 3. Seed Admin role ────────────────────────────────────────────
            var adminRole = await db.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Name == AdminRoleName, ct);

            if (adminRole is null)
            {
                adminRole = new Role
                {
                    Name        = AdminRoleName,
                    Description = "System administrator with full access",
                };
                db.Roles.Add(adminRole);
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Seeded 'Admin' role.");
            }

            // ── 4. Assign ALL permissions to the Admin role (idempotent) ─────
            var allPermissions     = await db.Permissions.ToListAsync(ct);
            var assignedIds        = adminRole.RolePermissions.Select(rp => rp.PermissionId).ToHashSet();
            var missingPermLinks   = allPermissions
                .Where(p => !assignedIds.Contains(p.Id))
                .Select(p => new RolePermission { RoleId = adminRole.Id, PermissionId = p.Id })
                .ToList();

            if (missingPermLinks.Count > 0)
            {
                db.RolePermissions.AddRange(missingPermLinks);
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Assigned {Count} permission(s) to Admin role.", missingPermLinks.Count);
            }

            // ── 5. Seed Admin user ────────────────────────────────────────────
            var adminUser = await db.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Username == AdminUsername, ct);

            if (adminUser is null)
            {
                adminUser = new User
                {
                    Username     = AdminUsername,
                    Email        = AdminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(AdminPassword),
                    IsActive     = true,
                };
                db.Users.Add(adminUser);
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Seeded admin user '{Username}'.", AdminUsername);
            }

            // ── 6. Assign Admin role to admin user (idempotent) ──────────────
            if (!adminUser.UserRoles.Any(ur => ur.RoleId == adminRole.Id))
            {
                db.UserRoles.Add(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Assigned 'Admin' role to admin user.");
            }

            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw; // Fail fast — a mis-configured DB should prevent startup
        }
    }
}
