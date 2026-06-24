using System.Collections.Concurrent;

namespace ApiAggregationService.Features.ApiAggregation.Services.Statistics;

public class StatisticsService : IStatisticsService
{
    private readonly ConcurrentDictionary<string, ApiStatisticsModel> _stats = new();

    public void Record(string apiName, long elapsedMilliseconds, bool isSuccess)
    {
        var stats = _stats.GetOrAdd(apiName, _ => new ApiStatisticsModel
        {
            ApiName = apiName
        });

        lock (stats)
        {
            stats.TotalRequests++;

            if (!isSuccess)
                stats.FailedRequests++;
            else
            {
                stats.TotalResponseTime += elapsedMilliseconds;

                if (elapsedMilliseconds < 100)
                    stats.FastRequests++;
                else if (elapsedMilliseconds <= 200)
                    stats.AverageRequests++;
                else
                    stats.SlowRequests++;
            }
        }
    }

    public IEnumerable<ApiStatisticsModel> GetStatistics()
    {
        return _stats.Values;
    }
}
