using FluentValidation;

namespace SoftlarePMS.Application.Features.Departments.Queries.GetDepartmentsWithPagination;

public sealed class GetDepartmentsWithPaginationQueryValidator : AbstractValidator<GetDepartmentsWithPaginationQuery>
{
    public GetDepartmentsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
