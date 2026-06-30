using ApiAggregationService.CommonMethods;
using ApiAggregationService.Features.ApiAggregation.ApiFilters;
using ApiAggregationService.Features.ApiAggregation.GetAggregatedData;
using ApiAggregationService.Features.ApiAggregation.GetStatistics;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregationService.Features.ApiAggregation.Services.Aggregation;

public interface IAggregationService
{
    Task<Result<GetAggregatedDataResponse>> GetAggregatedDataAsync(AggregatedDataFilter filter, CancellationToken ctct);
    Result<IReadOnlyList<GetRequestsStatistics>> GetRequestsStatistics(GenericFilter filter);
}
