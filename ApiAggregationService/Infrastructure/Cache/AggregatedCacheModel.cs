namespace ApiAggregationService.Infrastructure.Cache;

public record AggregatedCacheModel
{
    public decimal Value { get; init; }
    public DateTime Timestamp { get; init; }
    public int SourcesUsed { get; init; }
    public List<CachingValues> Providers { get; init; } = new();
}

public record CachingValues
{
    public string Provider { get; init; }
    public decimal Value { get; init; }
}
