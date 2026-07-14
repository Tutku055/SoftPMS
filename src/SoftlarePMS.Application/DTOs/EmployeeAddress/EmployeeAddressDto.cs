namespace SoftlarePMS.Application.DTOs.EmployeeAddress;

public record EmployeeAddressDto(
    Guid Id,
    string AddressTitle,
    string Country,
    string City,
    string District,
    string FullAddress,
    string ZipCode
);