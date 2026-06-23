using ApiAggregationService.CommonMethods;
using ApiAggregationService.Features.ApiAggregation.ApiFilters;
using ApiAggregationService.Features.ApiAggregation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregationService.Features.ApiAggregation.Controllers;

[ApiController]
[Route("api/aggregation")]
[Authorize]
public class PricesController : ControllerBase
{
    private readonly IAggregationService _aggregationService;
    public PricesController(IAggregationService AggregationService)
    {
        _aggregationService = AggregationService;
    }

    /// <summary>
    /// Get aggregated values
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAggregatedValues([FromQuery] AggregatedDataFilter filter, CancellationToken ct)
    {
        var aggregatedResult = await _aggregationService.GetAggregatedData(filter, ct);

        if (aggregatedResult.IsFailure)
        {
            return aggregatedResult.ReturnStates switch
            {
                ReturnStates.NotFound => NotFound(aggregatedResult.Error),
                _ => BadRequest(aggregatedResult.Error),
            };
        }

        return Ok(aggregatedResult.Value);
    }

    /// <summary>
    /// Get request's statistics
    /// </summary>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRequestsStatistics([FromQuery] GenericFilter filter)
    {
        var statisticsResult = await _aggregationService.GetRequestsStatistics(filter);

        if (statisticsResult.IsFailure)
            return BadRequest(statisticsResult.Error);

        return Ok(statisticsResult.Value);
    }
}