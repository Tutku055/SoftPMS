using FluentValidation;

namespace SoftlarePMS.Application.Features.Employees.Commands.UpdateEmployeeAddress;

/// <summary>FluentValidation rules for UpdateEmployeeAddressCommand.</summary>
public sealed class UpdateEmployeeAddressCommandValidator : AbstractValidator<UpdateEmployeeAddressCommand>
{
    public UpdateEmployeeAddressCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.AddressLine)
            .NotEmpty().WithMessage("Address line is required.")
            .MaximumLength(200).WithMessage("Address line must not exceed 200 characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");

        RuleFor(x => x.PostalCode)
            .MaximumLength(20).WithMessage("Postal code must not exceed 20 characters.");
    }
}
