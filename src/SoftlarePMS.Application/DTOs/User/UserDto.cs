using System;

namespace SoftlarePMS.Application.DTOs.User;

public record UserDto(
    Guid Id,
    Guid? EmployeeId,
    string Username,
    string Email,
    bool IsActive
);
