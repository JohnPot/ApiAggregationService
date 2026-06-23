using ApiAggregationService.ExternalApis;

namespace Tests;

public class FailingProvider : IApiProvider
{
    public string Name { get; }

    public FailingProvider(string name)
    {
        Name = name;
    }


    public Task<ExternalApiModel?> GetDataAsync(
        CancellationToken ct)
    {
        throw new Exception("API failed");
    }
}
