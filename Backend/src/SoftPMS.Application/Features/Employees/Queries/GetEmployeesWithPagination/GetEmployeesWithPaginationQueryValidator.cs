using FluentValidation;

namespace SoftPMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;

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
    }
}