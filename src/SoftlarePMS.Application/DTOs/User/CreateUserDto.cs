using System;

namespace SoftlarePMS.Application.DTOs.User;

public record CreateUserDto(
    Guid? EmployeeId,
    string Username,
    string Email,
    string Password,
    bool IsActive
);
