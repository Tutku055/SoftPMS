using System;

namespace SoftPMS.Application.DTOs.User;

public record UserDto(
    Guid Id,
    Guid? EmployeeId,
    string Username,
    string Email,
    bool IsActive
);
