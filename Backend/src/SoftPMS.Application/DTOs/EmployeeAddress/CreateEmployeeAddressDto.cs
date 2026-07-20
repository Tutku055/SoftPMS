namespace SoftPMS.Application.DTOs.EmployeeAddress;

public record CreateEmployeeAddressDto
{
    public string AddressLine { get; init; }
    public string Country { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string PostalCode { get; init; }
}