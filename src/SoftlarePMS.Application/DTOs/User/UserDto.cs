using System;

namespace SoftlarePMS.Application.DTOs.User;

public record UserDto(
    Guid Id,
    int InternalId,
    Guid? EmployeeId,
    string Username,
    string Email,
    bool IsActive
);
