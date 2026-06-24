using ApiAggregationService.ExternalApis;
using ApiAggregationService.Features.ApiAggregation.Services.Aggregation;
using ApiAggregationService.Features.ApiAggregation.Services.Statistics;
using ApiAggregationService.Features.ApiAggregation.Services.ValueTransformation;
using ApiAggregationService.Infrastructure.Resilience;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Tests;

public class ServiceFactory
{
    private readonly IMemoryCache _cache;
    private readonly IStatisticsService _statisticsService;
    private readonly IValueTransformation _valueTransformation;
    private readonly RetryPolicyFactory _retryPolicyFactory;
    private readonly ILogger<AggregationService> _logger;
    public ServiceFactory()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
        _statisticsService = new StatisticsService();
        _valueTransformation = new ValueTransformationService();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        _logger = loggerFactory.CreateLogger<AggregationService>();

        _retryPolicyFactory = new RetryPolicyFactory(loggerFactory.CreateLogger<RetryPolicyFactory>());
    }

    public AggregationService CreateAggregationService(
        IEnumerable<IApiProvider> providers)
    {
        return new AggregationService(
            providers,
            _valueTransformation,
            _cache,
            _statisticsService,
            _logger,
            _retryPolicyFactory);
    }
}
