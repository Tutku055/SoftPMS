namespace SoftlarePMS.Application.DTOs.EmployeeReference;

public record CreateEmployeeReferenceDto(
    string FullName,
    string Company,
    string Title,
    string PhoneNumber,
    string Email
);