using SoftlarePMS.Application.Common.Interfaces;

namespace SoftlarePMS.Infrastructure.Services;

public sealed class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
