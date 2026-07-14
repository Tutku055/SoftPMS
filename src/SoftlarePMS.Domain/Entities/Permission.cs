namespace SoftlarePMS.Domain.Entities;

public class Permission: BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
}
