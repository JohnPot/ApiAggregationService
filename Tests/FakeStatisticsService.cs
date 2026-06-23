using ApiAggregationService.Services.Statistics;

namespace Tests;

public class FakeStatisticsService : IStatisticsService
{
    public void Record(string name, long milliseconds, bool success)
    {
    }

    public List<ApiStatistics> GetStatistics()
    {
        return new();
    }
}
