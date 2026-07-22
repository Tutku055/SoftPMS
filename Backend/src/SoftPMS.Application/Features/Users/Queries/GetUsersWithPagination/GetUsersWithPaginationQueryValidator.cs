using FluentValidation;

namespace SoftPMS.Application.Features.Users.Queries.GetUsersWithPagination;

public sealed class GetUsersWithPaginationQueryValidator
    : AbstractValidator<GetUsersWithPaginationQuery>
{
    private const int MaxPageSize = 200;

    public GetUsersWithPaginationQueryValidator()
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
