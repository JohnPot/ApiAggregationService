using ApiAggregationService.CommonMethods;
using ApiAggregationService.Infrastructure.Cache;

namespace ApiAggregationService.Features.ApiAggregation.GetAggregatedData;
public static class AggregatedDataMapper
{
    public static GetAggregatedDataResponse ToResponse(this AggregatedCacheModel cached, SourceEnums sourceEnums)
    {
        return new GetAggregatedDataResponse
        {
            AverageValue = cached.Value,
            TimeFetched = cached.Timestamp,
            DataSource = Enum.GetName(sourceEnums),
            SourcesUsed = cached.SourcesUsed,
            Providers = cached.Providers
                .Select(x => new ProviderValue
                {
                    Provider = x.Provider,
                    Value = x.Value
                })
                .ToList()
        };
    }

    public static AggregatedCacheModel ToCacheModel(this GetAggregatedDataResponse response)
    {
        return new AggregatedCacheModel
        {
            Value = response.AverageValue,
            Timestamp = response.TimeFetched,
            SourcesUsed = response.SourcesUsed,
            Providers = response.Providers
                .Select(x => new CachingValues
                {
                    Provider = x.Provider,
                    Value = x.Value
                })
                .ToList()
        };
    }
}