using FluentValidation;

namespace SoftlarePMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;

/// <summary>
/// Validates pagination parameters before they reach PaginatedList.CreateAsync,
/// preventing DivideByZeroException (pageSize == 0) and negative Skip calculations.
/// </summary>
public sealed class GetEmployeesWithPaginationQueryValidator
    : AbstractValidator<GetEmployeesWithPaginationQuery>
{
    private const int MaxPageSize = 200;

    public GetEmployeesWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1.")
            .LessThanOrEqualTo(MaxPageSize)
            .WithMessage($"Page size must not exceed {MaxPageSize}.");

        RuleFor(x => x.MinRate)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinRate.HasValue)
            .WithMessage("Minimum rate must be a non-negative value.");

        RuleFor(x => x.MaxRate)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxRate.HasValue)
            .WithMessage("Maximum rate must be a non-negative value.");

        RuleFor(x => x)
            .Must(x => !x.MinRate.HasValue || !x.MaxRate.HasValue || x.MinRate <= x.MaxRate)
            .WithMessage("MinRate must be less than or equal to MaxRate.");
    }
}
