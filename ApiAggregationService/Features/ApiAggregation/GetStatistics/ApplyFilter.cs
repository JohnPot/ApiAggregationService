using ApiAggregationService.Features.ApiAggregation.ApiFilters;

namespace ApiAggregationService.Features.ApiAggregation.GetStatistics;

public static class StatisticsFilterExtensions
{
    public static List<GetRequestsStatistics> ApplyFilter(this IEnumerable<GetRequestsStatistics> statistics, AggregatedDataFilter filter)
    {
        var query = statistics;

        query = filter.Sort?.ToLower() switch
        {
            "asc" => query.OrderBy(x => x.ApiName),
            "desc" => query.OrderByDescending(x => x.ApiName),
            _ => query
        };

        return query.ToList();
    }
}