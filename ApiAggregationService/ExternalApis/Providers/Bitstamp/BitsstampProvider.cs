namespace ApiAggregationService.ExternalApis.Providers.Bitstamp;

public class BitstampProvider : IApiProvider
{
    private readonly HttpClient _httpClient;

    public BitstampProvider(HttpClient HttpClient)
    {
        _httpClient = HttpClient;
    }

    public string Name => "Bitstamp";

    public async Task<ExternalApiModel> GetDataAsync(CancellationToken ct)
    {
        var unix = new DateTimeOffset(DateTime.UtcNow.Date.AddMinutes(DateTime.UtcNow.Minute)).ToUnixTimeSeconds();

        var response = await _httpClient.GetFromJsonAsync<BitstampOhlcResponse>($"api/v2/ohlc/btcusd/?step=3600&limit=1&start={unix}", ct);

        var volume = response?.Data?.Ohlc[0].Volume;

        if (volume is null)
            return null;

        return new ExternalApiModel
        {
            ApiValue = (decimal)volume,
            Provider = Name
        };
    }
}
