using ApiAggregationService.Features.ApiAggregation.Services.Statistics;

namespace ApiAggregationService.BackgroundWorkers;

public class ServiceAnalyzer : BackgroundService
{
    private readonly IStatisticsService _statistics;
    private readonly ILogger<ServiceAnalyzer> _logger;
    private const int AnomalyThreshold = 300;


    public ServiceAnalyzer(IStatisticsService statistics, ILogger<ServiceAnalyzer> logger)
    {
        _statistics = statistics;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var stats = _statistics.GetStatistics();

            foreach (var stat in stats)
            {
                var isAnomaly = stat.AverageResponseTime > AnomalyThreshold;

                if (!stat.hasAnomaly && isAnomaly)
                {
                    _logger.LogWarning("{ExternalApi} performance anomaly detected, average response time is {AverageResponseTime}ms", stat.ApiName, stat.AverageResponseTime);
                    stat.hasAnomaly = true;
                }

                if(!isAnomaly)
                    stat.hasAnomaly = false;
            }


            await Task.Delay(TimeSpan.FromMinutes(1), ct);
        }
    }
}
