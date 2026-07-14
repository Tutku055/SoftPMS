using System;

namespace SoftlarePMS.Application.DTOs.User;

public record UpdateUserDto(
    Guid? EmployeeId,
    string Username,
    string Email,
    bool IsActive
);
