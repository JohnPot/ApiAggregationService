namespace ApiAggregationService.ExternalApis.Providers.Github;

public class GithubProvider : IApiProvider
{
    private readonly HttpClient _httpClient;


    public GithubProvider(HttpClient HttpClient)
    {
        _httpClient = HttpClient;
    }

    public string Name => "Github";
    public async Task<ExternalApiModel> GetDataAsync(CancellationToken ct)
    {
        var response = await _httpClient.GetFromJsonAsync<GithubRepositoryResponse>("repos/dotnet/runtime", ct);

        if (response is null)
            return null;

        return new ExternalApiModel
        {
            ApiValue = response.ForksCount,
            Provider = Name
        };
    }
}
