using ApiAggregationService.CommonMethods;
using ApiAggregationService.ExternalApis;
using ApiAggregationService.Features.ApiAggregation.ApiFilters;
using ApiAggregationService.Features.ApiAggregation.GetAggregatedData;
using ApiAggregationService.Features.ApiAggregation.GetStatistics;
using ApiAggregationService.Features.ApiAggregation.Interfaces;
using ApiAggregationService.Infrastructure.Cache;
using ApiAggregationService.Infrastructure.Resilience;
using ApiAggregationService.Services.Statistics;
using ApiAggregationService.Services.ValueTransformation;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using System.Diagnostics;

namespace ApiAggregationService.Features.ApiAggregation.Services;

public class AggregationService : IAggregationService
{
    private readonly IEnumerable<IApiProvider> _providers;
    private readonly IValueTransformation _valueTransformation;
    private readonly IMemoryCache _cache;
    private readonly IStatisticsService _statisticsService;
    private readonly ILogger<AggregationService> _logger;
    private readonly RetryPolicyFactory _retryPolicyFactory;

    private const string CacheKey = "aggregated_data";
    public AggregationService(IEnumerable<IApiProvider> providers, IValueTransformation ValueTransformation, IMemoryCache cache, 
                              IStatisticsService statisticsService, ILogger<AggregationService> logger, RetryPolicyFactory retryPolicyFactory)
    {
        _providers = providers;
        _valueTransformation = ValueTransformation;
        _cache = cache;
        _statisticsService = statisticsService;
        _logger = logger;
        _retryPolicyFactory = retryPolicyFactory;
    }

    public async Task<Result<GetAggregatedDataResponse>> GetAggregatedData(AggregatedDataFilter filter, CancellationToken ct)
    {
        //TRY TO FETCH FROM CACHE//
        var cached = _cache.Get<AggregatedCacheModel>(CacheKey);
        var currentMinute = DateTime.UtcNow.Date.AddMinutes(DateTime.UtcNow.Minute);

        if (cached is not null && cached.Timestamp == currentMinute)
        {
            var response = cached.ToResponse(SourceEnums.InMemory);
            response.Providers = response.Providers.ApplyFilter(filter);

            return Result<GetAggregatedDataResponse>.Success(response);
        }

        //FETCH DATA FROM THE EXTERNAL PROVIDERS//
        var tasks = _providers.Select(async provider =>
        {
            var sw = Stopwatch.StartNew();
            bool isSuccess = true;
            try
            {
                //RETRY IF EXCEPTION OCCURS//
                return await _retryPolicyFactory.CreateExternalApiRetry().ExecuteAsync(
                    async () =>
                    {
                        _logger.LogInformation("Fetching data from API {Provider}", provider.Name);
                        var data =  await provider.GetDataAsync(ct);

                        _logger.LogInformation("Successfully fetched data from API {Provider}", provider.Name);
                        return data;
                    });

            }
            catch (Exception ex)
            {
                isSuccess = false;
                _logger.LogError(ex, "Provider {Provider} failed", provider.Name);
                return null;
            }
            finally
            {
                sw.Stop();
                _statisticsService.Record(provider.Name, sw.ElapsedMilliseconds, isSuccess);
            }
        });

        //RUN CONCURRENTLY//
        var finishedTasks = await Task.WhenAll(tasks);

        List<ExternalApiModel?> values = finishedTasks
            .Where(x => x != null)
            .ToList();

        //FALLBACK, TRY AND GET FROM CACHE OR RETURN ERROR//
        if (!values.Any())
        {
            _logger.LogWarning("All external APIs failed");

            cached = _cache.Get<AggregatedCacheModel>(CacheKey);

            if (cached is not null)
            {
                _logger.LogInformation("Returning cached data fallback");

                var response = cached.ToResponse(SourceEnums.InMemory);
                response.Providers = response.Providers.ApplyFilter(filter);

                return Result<GetAggregatedDataResponse>.Success(response);
            }

            _logger.LogError("All providers are unavailable and no cache exists");

            return Result<GetAggregatedDataResponse>.Failure("No external APIs available", ReturnStates.NotFound);
        }

        //CREATE MODEL TO STORE IN CACHE AND RETURN//
        GetAggregatedDataResponse AggregatedData = new()
        {
            TimeFetched = DateTime.UtcNow.Date.AddMinutes(DateTime.UtcNow.Minute),
            AggregatedValue = _valueTransformation.Formula(values),
            DataSource = Enum.GetName(SourceEnums.ExternalAPI),
            SourcesUsed = values.Count(),
            Providers = values.Select(x => new ProviderValue()
            {
                Provider = x.Provider,
                Value = x.ApiValue
            }).ToList()
        };

        //ADD VALUES TO CACHE//
        _cache.Set(CacheKey, AggregatedData.ToCacheModel(), TimeSpan.FromHours(1));
        _logger.LogInformation("Aggregated data cached for {Duration}", TimeSpan.FromHours(1));

        //APPLY FILTERS//
        AggregatedData.Providers = AggregatedData.Providers.ApplyFilter(filter);

        return Result<GetAggregatedDataResponse>.Success(AggregatedData);
    }

    public async Task<Result<List<GetRequestsStatistics>>> GetRequestsStatistics(GenericFilter filter)
    {
        var statistics = _statisticsService.GetStatistics();

        //APPLY FILTERS//
        var response = statistics.ToResponse();
        response = response.ApplyFilter(filter);

        return Result<List<GetRequestsStatistics>>.Success(response);
    }
}