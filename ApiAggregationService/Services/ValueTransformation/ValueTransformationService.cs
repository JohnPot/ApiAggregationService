using ApiAggregationService.ExternalApis;

namespace ApiAggregationService.Services.ValueTransformation;

public class ValueTransformationService : IValueTransformation
{
    public decimal Formula(IEnumerable<ExternalApiModel> values)
    {
        var valuesList = values.ToList();

        if (!values.Any())
        {
            throw new InvalidOperationException("Cannot aggregate an empty collection of prices.");
        }

        return values.Average(x => x.ApiValue);
    }
}
