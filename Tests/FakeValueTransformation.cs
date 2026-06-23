using ApiAggregationService.ExternalApis;
using ApiAggregationService.Services.ValueTransformation;

namespace Tests;

public class FakeValueTransformation : IValueTransformation
{
    public decimal Formula(IEnumerable<ExternalApiModel> values)
    {
        return values
            .Where(x => x != null)
            .Sum(x => x!.ApiValue);
    }
}
