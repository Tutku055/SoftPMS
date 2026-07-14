namespace SoftlarePMS.Application.DTOs.EmployeeAddress;

public record UpdateEmployeeAddressDto(
    string AddressTitle,
    string Country,
    string City,
    string District,
    string FullAddress,
    string ZipCode
);
