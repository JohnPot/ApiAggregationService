using ApiAggregationService.CommonMethods;
using ApiAggregationService.Features.ApiAggregation.ApiFilters;

namespace ApiAggregationService.Features.ApiAggregation.GetAggregatedData;

public static class AggregatedDataFilterExtensions
{
    public static List<ProviderValue> ApplyFilter(this IEnumerable<ProviderValue> providers, AggregatedDataFilter filter)
    {
        var query = providers;

        query = filter.SortBy switch
        {
            AggregatedDataSortBy.Value => filter.Direction == SortDirection.Desc
               ? query.OrderByDescending(x => x.Value)
               : query.OrderBy(x => x.Value),

            AggregatedDataSortBy.Provider => filter.Direction == SortDirection.Desc
                        ? query.OrderByDescending(x => x.Provider)
                        : query.OrderBy(x => x.Provider),

            _ => query
        };

        return query.ToList();
    }
}