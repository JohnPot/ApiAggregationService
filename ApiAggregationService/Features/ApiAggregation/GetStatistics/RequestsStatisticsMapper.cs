using ApiAggregationService.Services.Statistics;

namespace ApiAggregationService.Features.ApiAggregation.GetStatistics;

public static class RequestsStatisticsMapper
{
    public static List<GetRequestsStatistics> ToResponse(this IEnumerable<ApiStatistics> statistics)
    {
        return statistics
            .Select(statistic => new GetRequestsStatistics
            {
                ApiName = statistic.ApiName,
                TotalRequests = statistic.TotalRequests,
                FailedRequests = statistic.FailedRequests,
                FastRequests = statistic.FastRequests,
                AverageRequests = statistic.AverageRequests,
                SlowRequests = statistic.SlowRequests,
                AverageResponseTime = statistic.AverageResponseTime
            })
            .ToList();
    }
}