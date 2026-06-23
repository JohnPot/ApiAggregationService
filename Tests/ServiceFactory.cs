using ApiAggregationService.ExternalApis;
using ApiAggregationService.Features.ApiAggregation.Services;
using ApiAggregationService.Services.Statistics;
using ApiAggregationService.Services.ValueTransformation;
using Microsoft.Extensions.Caching.Memory;

namespace Tests;

public class ServiceFactory
{
    private readonly IMemoryCache _cache;
    private readonly IStatisticsService _statisticsService;
    private readonly IValueTransformation _valueTransformation;
    public ServiceFactory()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
        _statisticsService = new FakeStatisticsService();
        _valueTransformation = new FakeValueTransformation();
    }

    public AggregationService CreateAggregationService(
        IEnumerable<IApiProvider> providers)
    {
        return new AggregationService(
            providers,
            _valueTransformation,
            _cache,
            _statisticsService);
    }
}
