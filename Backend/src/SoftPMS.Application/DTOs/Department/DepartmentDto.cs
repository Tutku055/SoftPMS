namespace SoftPMS.Application.DTOs.Department;

public record DepartmentDto(
    Guid Id,
    string Name,
    string Description,
    int EmployeeCount
);
