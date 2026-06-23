using ApiAggregationService.ExternalApis;

namespace Tests;

public class FakeProvider : IApiProvider
{
    private readonly decimal _value;
    public string Name { get; }
    public int CallCount { get; private set; }


    public FakeProvider(string name, decimal value)
    {
        Name = name;
        _value = value;
    }


    public Task<ExternalApiModel> GetDataAsync(CancellationToken ct)
    {
        CallCount++;

        return Task.FromResult<ExternalApiModel>(
            new ExternalApiModel
            {
                Provider = Name,
                ApiValue = _value
            });
    }
}