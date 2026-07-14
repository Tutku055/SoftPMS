namespace SoftlarePMS.Application.DTOs.EmployeeReference;

public record EmployeeReferenceDto(
    Guid Id,
    string FullName,
    string Company,
    string Title,
    string PhoneNumber,
    string Email
);