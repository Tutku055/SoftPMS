using SoftPMS.Application.Common.Interfaces;

namespace SoftPMS.Infrastructure.Services;

public sealed class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
