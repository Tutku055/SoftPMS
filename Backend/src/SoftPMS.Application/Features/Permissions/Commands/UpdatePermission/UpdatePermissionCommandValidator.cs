using FluentValidation;

namespace SoftPMS.Application.Features.Permissions.Commands.UpdatePermission;

public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Permission ID is required.");

        RuleFor(v => v.Dto.Name)
            .NotEmpty().WithMessage("Permission name is required.")
            .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");

        RuleFor(v => v.Dto.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
