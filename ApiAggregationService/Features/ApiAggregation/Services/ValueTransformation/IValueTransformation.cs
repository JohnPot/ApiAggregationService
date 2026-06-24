using ApiAggregationService.ExternalApis;

namespace ApiAggregationService.Features.ApiAggregation.Services.ValueTransformation;

public interface IValueTransformation
{
    decimal Formula(IEnumerable<ExternalApiModel> values);
}
