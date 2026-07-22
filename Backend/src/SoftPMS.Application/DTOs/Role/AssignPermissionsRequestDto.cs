namespace SoftPMS.Application.DTOs.Role;

/// <summary>Request body for the assign-permissions endpoint.</summary>
public sealed record AssignPermissionsRequestDto(List<Guid> PermissionIds);
