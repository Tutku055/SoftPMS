namespace SoftPMS.Application.DTOs.Role;

public record RoleDto(
    Guid Id,
    string Name,
    string Description,
    string Color
);