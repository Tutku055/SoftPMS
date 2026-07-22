namespace SoftPMS.Application.DTOs.Users;

/// <summary>Request body for the assign-role endpoint.</summary>
public sealed record AssignRoleRequestDto(Guid RoleId);
