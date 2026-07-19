namespace SoftlarePMS.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
}
