using FluentValidation;

namespace SoftlarePMS.Application.Features.Permissions.Commands.CreatePermission;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(v => v.Dto.Name)
            .NotEmpty().WithMessage("Permission name is required.")
            .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");

        RuleFor(v => v.Dto.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
