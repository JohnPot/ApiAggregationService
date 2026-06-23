using ApiAggregationService.CommonMethods;

namespace ApiAggregationService.Features.ApiAggregation.ApiFilters;

public class AggregatedDataFilter : GenericFilter
{
    public AggregatedDataSortBy? SortBy { get; init; }
}

public class GenericFilter
{
    public SortDirection? Direction { get; init; }
}