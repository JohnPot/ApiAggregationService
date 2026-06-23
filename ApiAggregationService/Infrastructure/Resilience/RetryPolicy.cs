using Polly;

namespace ApiAggregationService.Infrastructure.Resilience;

public class RetryPolicyFactory
{
    private readonly ILogger<RetryPolicyFactory> _logger;

    public RetryPolicyFactory(ILogger<RetryPolicyFactory> logger)
    {
        _logger = logger;
    }

    public IAsyncPolicy CreateExternalApiRetry()
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromMilliseconds(500 * retryAttempt),
                (exception, delay, retryCount, context) =>
                {
                    _logger.LogWarning(exception, "Retry attempt {RetryCount} after {Delay}ms", retryCount, delay.TotalMilliseconds);
                });
    }
}