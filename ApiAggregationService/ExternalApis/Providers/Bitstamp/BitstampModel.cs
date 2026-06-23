namespace ApiAggregationService.ExternalApis.Providers.Bitstamp;

public record BitstampOhlcResponse
{
    public BitstampOhlcData Data { get; init; } = new();
}


public record BitstampOhlcData
{
    public string Pair { get; init; } = string.Empty;

    public List<BitstampCandle> Ohlc { get; init; } = new();
}


public record BitstampCandle
{
    public long Timestamp { get; init; }

    public decimal Open { get; init; }

    public decimal High { get; init; }

    public decimal Low { get; init; }

    public decimal Close { get; init; }

    public decimal Volume { get; init; }
}