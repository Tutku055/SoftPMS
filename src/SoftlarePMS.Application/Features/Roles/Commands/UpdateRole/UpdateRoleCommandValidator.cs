using FluentValidation;

namespace SoftlarePMS.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Role ID is required.");

        RuleFor(v => v.Dto.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");

        RuleFor(v => v.Dto.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
