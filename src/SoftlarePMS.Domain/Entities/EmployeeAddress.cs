namespace SoftlarePMS.Domain.Entities;

public class EmployeeAddress : BaseEntity
{
    public Guid EmployeeId { get; set; }

    public string AddressLine { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public bool IsPrimary { get; set; } = false;

    // Historic tracking: when this address record became active
    public DateTime StartDate { get; set; }

    // Historic tracking: null means this is the current active address
    public DateTime? EndDate { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}
