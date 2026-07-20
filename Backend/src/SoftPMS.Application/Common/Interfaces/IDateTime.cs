namespace SoftPMS.Application.Common.Interfaces;

/// <summary>
/// Abstracts system time so handlers can be unit-tested with controlled timestamps.
/// </summary>
public interface IDateTime
{
    /// <summary>Returns the current UTC date and time.</summary>
    DateTime UtcNow { get; }
}
