namespace SoftPMS.Application.DTOs.Role;

public record RoleListDto(
    Guid Id,
    string Name,
    string Description,
    string Color,
    int UserCount,
    bool IsActive,
    bool IsSystemRole
);
