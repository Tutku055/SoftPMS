using FluentValidation.Results;

namespace SoftlarePMS.Application.Common.Exceptions;

/// <summary>
/// Application-layer exception that aggregates FluentValidation failures into a structured
/// dictionary keyed by field name. This is distinct from Domain.Exceptions.ValidationException
/// (which holds a plain string message); this version is required by the ValidationBehavior
/// pipeline to surface multiple per-field errors to the API response layer.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>Field-keyed dictionary of validation error messages.</summary>
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures occurred.")
    {
        // Group failures by property name so the client gets all errors per field at once.
        Errors = failures
            .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }
}
