using System.Collections.Concurrent;

namespace ApiAggregationService.Services.Statistics;

public class StatisticsService : IStatisticsService
{
    private readonly ConcurrentDictionary<string, ApiStatistics> _stats = new();

    public void Record(string apiName, long elapsedMilliseconds, bool isSuccess)
    {
        var stats = _stats.GetOrAdd(apiName, _ => new ApiStatistics
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

    public List<ApiStatistics> GetStatistics()
    {
        return _stats.Values.ToList();
    }
}
