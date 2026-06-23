namespace ApiAggregationService.Features.ApiAggregation.GetAggregatedData;

public record GetAggregatedDataResponse
{
    public string DataSource { get; init; }
    public DateTime TimeFetched { get; init; }
    public decimal AggregatedValue { get; init; }
    public int SourcesUsed { get; init; }
    public List<ProviderValue> Providers { get; set; } = new();
}

public record ProviderValue
{
    public string Provider { get; init; }
    public decimal Value { get; init; }
}

