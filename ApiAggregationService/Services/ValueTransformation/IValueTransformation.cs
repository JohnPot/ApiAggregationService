using ApiAggregationService.ExternalApis;

namespace ApiAggregationService.Services.ValueTransformation;

public interface IValueTransformation
{
    decimal Formula(IEnumerable<ExternalApiModel> values);
}
