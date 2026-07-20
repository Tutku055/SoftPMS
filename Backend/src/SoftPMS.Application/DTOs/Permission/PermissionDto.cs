namespace SoftPMS.Application.DTOs.Permission;

public record PermissionDto(
    Guid Id,
    string Name,
    string Description
);
