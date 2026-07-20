namespace SoftPMS.Application.DTOs.EmployeeReference;

public record UpdateEmployeeReferenceDto(
    string FullName,
    string Company,
    string Title,
    string PhoneNumber,
    string Email,
    string Notes
);
