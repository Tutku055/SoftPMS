namespace SoftPMS.Domain.Entities;

public class Role: BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public bool IsSystemRole { get; set; } = false;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
}
