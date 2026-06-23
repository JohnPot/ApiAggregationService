namespace ApiAggregationService.Services.Statistics;

public class ApiStatistics
{
    public string ApiName { get; set; }
    public long TotalRequests { get; set; }
    public long FailedRequests { get; set; }
    public long TotalResponseTime { get; set; }
    public long FastRequests { get; set; }
    public long AverageRequests { get; set; }
    public long SlowRequests { get; set; }
    public decimal AverageResponseTime => TotalRequests == 0 ? 0 : (decimal)TotalResponseTime / TotalRequests;
    public bool hasAnomaly { get; set; }
}
