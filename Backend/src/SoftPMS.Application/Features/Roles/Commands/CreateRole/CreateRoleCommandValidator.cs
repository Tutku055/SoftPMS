using FluentValidation;

namespace SoftPMS.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(v => v.Dto.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");

        RuleFor(v => v.Dto.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
