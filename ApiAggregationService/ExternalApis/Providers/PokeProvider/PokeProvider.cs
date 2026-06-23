namespace ApiAggregationService.ExternalApis.Providers.PokeProvider;

public class PokeProvider : IApiProvider
{
    private readonly HttpClient _httpClient;

    public PokeProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string Name => "Poke";

    public async Task<ExternalApiModel> GetDataAsync(CancellationToken ct)
    {
        var url = $"api/v2/pokemon";

        var response = await _httpClient.GetFromJsonAsync<PokemonListResponse>(url, ct);

        var count = response.Count;

        return new ExternalApiModel()
        {
            ApiValue = count,
            Provider = Name
        };
    }


}
