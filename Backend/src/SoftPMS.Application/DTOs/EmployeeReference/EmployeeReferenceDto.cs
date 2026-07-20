namespace SoftPMS.Application.DTOs.EmployeeReference;

public record EmployeeReferenceDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Company { get; init; }
    public string Title { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
}