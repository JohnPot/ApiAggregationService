namespace ApiAggregationService.Services.Statistics;

public interface IStatisticsService
{
    void Record(string apiName, long elapsedMilliseconds, bool isSuccess);
    List<ApiStatistics> GetStatistics();
}