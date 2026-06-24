namespace ApiAggregationService.Features.ApiAggregation.Services.Statistics;

public interface IStatisticsService
{
    void Record(string apiName, long elapsedMilliseconds, bool isSuccess);
    IEnumerable<ApiStatisticsModel> GetStatistics();
}