namespace ApiAggregationService.ExternalApis.Providers;

public class BitfinexProvider : IApiProvider
{
    private readonly HttpClient _httpClient;


    public BitfinexProvider(HttpClient HttpClient)
    {
        _httpClient = HttpClient;
    }

    public string Name => "Bitfinex";

    public async Task<ExternalApiModel> GetDataAsync(CancellationToken ct)
    {
        var start = new DateTimeOffset(DateTime.UtcNow.Date.AddHours(DateTime.UtcNow.Hour)).ToUnixTimeMilliseconds();
        var end = start + 3600000;

        var url = $"v2/candles/trade:1h:tBTCUSD/hist?start={start}&end={end}&limit=1";

        var response = await _httpClient.GetFromJsonAsync<decimal[][]>(url, ct);

        var close = response![0][4];

        return new ExternalApiModel
        {
            ApiValue = close,
            Provider = Name
        };
    }
}
