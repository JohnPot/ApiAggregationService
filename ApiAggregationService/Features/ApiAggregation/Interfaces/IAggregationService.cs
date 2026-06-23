using ApiAggregationService.CommonMethods;
using ApiAggregationService.Features.ApiAggregation.ApiFilters;
using ApiAggregationService.Features.ApiAggregation.GetAggregatedData;
using ApiAggregationService.Features.ApiAggregation.GetStatistics;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregationService.Features.ApiAggregation.Interfaces;

public interface IAggregationService
{
    Task<Result<GetAggregatedDataResponse>> GetAggregatedData(AggregatedDataFilter filter, CancellationToken ctct);
    Task<Result<List<GetRequestsStatistics>>> GetRequestsStatistics(GenericFilter filter);
}
