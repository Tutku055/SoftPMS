namespace SoftPMS.Domain.Entities;

public class User : BaseEntity
{
    public Guid? EmployeeId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation properties
    public virtual Employee? Employee { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

    public virtual ICollection<Employee> CreatedEmployees { get; set; } = new HashSet<Employee>();

    public virtual ICollection<EmployeeCompensation> CreatedCompensations { get; set; } = new HashSet<EmployeeCompensation>();

    public virtual ICollection<Document> CreatedDocuments { get; set; } = new HashSet<Document>();

    public virtual ICollection<EmployeeNote> CreatedNotes { get; set; } = new HashSet<EmployeeNote>();
}

