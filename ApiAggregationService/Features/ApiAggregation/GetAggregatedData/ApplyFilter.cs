using ApiAggregationService.Features.ApiAggregation.ApiFilters;

namespace ApiAggregationService.Features.ApiAggregation.GetAggregatedData;

public static class AggregatedDataFilterExtensions
{
    public static List<ProviderValue> ApplyFilter(this IEnumerable<ProviderValue> providers, AggregatedDataFilter filter)
    {
        var query = providers;

        query = filter.Sort?.ToLower() switch
        {
            "value_asc" => query.OrderBy(x => x.Value),
            "value_desc" => query.OrderByDescending(x => x.Value),
            "provider_asc" => query.OrderBy(x => x.Provider),
            "provider_desc" => query.OrderByDescending(x => x.Provider),
            _ => query
        };

        return query.ToList();
    }
}