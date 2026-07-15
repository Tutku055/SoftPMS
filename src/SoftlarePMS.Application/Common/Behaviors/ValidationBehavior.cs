using FluentValidation;
using MediatR;

namespace SoftlarePMS.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that runs all registered FluentValidation validators
/// for a request before the handler executes. Throws Application.Common.Exceptions.ValidationException
/// if any failures are detected, preventing invalid data from reaching the handler.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        // Run all validators in parallel for performance
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count > 0)
            throw new Exceptions.ValidationException(failures);

        return await next(cancellationToken);
    }
}
