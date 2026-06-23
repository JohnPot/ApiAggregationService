namespace ApiAggregationService.ExternalApis;

public record ExternalApiModel
{
    public string Provider { get; init; }
    public decimal ApiValue { get; init; }
}
