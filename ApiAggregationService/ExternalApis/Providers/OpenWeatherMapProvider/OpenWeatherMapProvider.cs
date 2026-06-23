namespace ApiAggregationService.ExternalApis.Providers.OpenWeatherMapProvider;

public class OpenWeatherMapProvider : IApiProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public OpenWeatherMapProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public string Name => "OpenWeather";
    public async Task<ExternalApiModel> GetDataAsync(CancellationToken ct)
    {
        var url = $"data/2.5/weather?q=Thessaloniki&appid={_configuration["ExternalApiKeys:WeatherApiKey"]}&units=metric";

        var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(url, ct);

        return new ExternalApiModel()
        {
            ApiValue = (decimal)response.Main.Temp,
            Provider = Name
        };

    }


}
