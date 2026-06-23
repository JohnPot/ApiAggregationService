namespace ApiAggregationService.ExternalApis.Providers.PokeProvider;

public record PokemonListResponse
{
    public int Count { get; init; }

    public string Next { get; init; }

    public string Previous { get; init; }

    public List<PokemonResult> Results { get; init; }
}

public record PokemonResult
{
    public string Name { get; init; }

    public string Url { get; init; }
}