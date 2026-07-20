namespace SoftPMS.Application.DTOs.EmployeeAddress;

public record EmployeeAddressDto
{
    public Guid Id { get; init; }
    public string AddressLine { get; init; }
    public string Country { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string PostalCode { get; init; }
    public bool IsPrimary { get; init; }
}