using System;

namespace SoftPMS.Application.DTOs.User;

public record UserDto(
    Guid Id,
    Guid? EmployeeId,
    string Username,
    string Email,
    bool IsActive,
    bool IsSystemUser,
    bool RequiresPasswordChange,
    SoftPMS.Application.DTOs.Role.RoleDto? Role
);
