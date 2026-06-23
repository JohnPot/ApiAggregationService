namespace ApiAggregationService.Features.ApiAggregation.GetStatistics;

public record GetRequestsStatistics
{
    public string ApiName { get; init; }
    public long TotalRequests { get; init; }
    public long FailedRequests { get; init; }
    public long FastRequests { get; init; }
    public long AverageRequests { get; init; }
    public long SlowRequests { get; init; }
    public decimal AverageResponseTime { get; init; }
}
