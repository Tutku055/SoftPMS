using System;

namespace SoftPMS.Application.DTOs.User;

public record CreateUserDto(
    Guid? EmployeeId,
    string Username,
    string Email,
    string Password,
    Guid RoleId,
    bool IsActive
);
