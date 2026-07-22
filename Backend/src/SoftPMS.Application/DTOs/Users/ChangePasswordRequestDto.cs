namespace SoftPMS.Application.DTOs.Users;

/// <summary>Request body for the change-password endpoint.</summary>
public sealed record ChangePasswordRequestDto(string OldPassword, string NewPassword);
