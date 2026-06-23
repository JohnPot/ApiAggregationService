namespace ApiAggregationService.ExternalApis;

public interface IApiProvider
{
    string Name { get; }
    Task<ExternalApiModel> GetDataAsync(CancellationToken ct);
}