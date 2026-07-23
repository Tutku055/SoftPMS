namespace SoftPMS.Application.DTOs.Role;

public record CreateRoleDto(
    string Name,
    string Description,
    string Color,
    List<Guid>? PermissionIds = null
);