using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SoftPMS.Application.Features.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    Guid UserId,
    [Required] string OldPassword,
    [Required] string NewPassword
) : IRequest<Unit>;
