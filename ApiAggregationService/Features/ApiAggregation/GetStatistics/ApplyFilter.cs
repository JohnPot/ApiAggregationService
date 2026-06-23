using ApiAggregationService.CommonMethods;
using ApiAggregationService.Features.ApiAggregation.ApiFilters;

namespace ApiAggregationService.Features.ApiAggregation.GetStatistics;

public static class StatisticsFilterExtensions
{
    public static List<GetRequestsStatistics> ApplyFilter(this IEnumerable<GetRequestsStatistics> statistics, GenericFilter filter)
    {
        var query = statistics;

        query = filter.Direction switch
        {
            SortDirection.Desc => query.OrderByDescending(x => x.ApiName),
            SortDirection.Asc => query.OrderBy(x => x.ApiName),
            _ => query
        };

        return query.ToList();
    }
}