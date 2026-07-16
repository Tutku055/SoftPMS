using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SoftlarePMS.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs the start, completion, and elapsed time of every request.
/// Emits a warning when a request takes longer than 500 ms (potential performance issue).
/// </summary>
public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int SlowRequestThresholdMs = 500;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Handling {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();
        TResponse response;

        try
        {
            response = await next(cancellationToken);
        }
        finally
        {
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > SlowRequestThresholdMs)
            {
                logger.LogWarning(
                    "Slow request detected: {RequestName} took {ElapsedMs} ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogInformation(
                    "Handled {RequestName} in {ElapsedMs} ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);
            }
        }

        return response;
    }
}
