namespace SoftlarePMS.Application.DTOs.EmployeeAddress;

public record CreateEmployeeAddressDto(
    string AddressTitle,
    string Country,
    string City,
    string District,
    string FullAddress,
    string ZipCode
);