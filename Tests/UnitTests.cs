using ApiAggregationService.CommonMethods;
using ApiAggregationService.ExternalApis;
using ApiAggregationService.Features.ApiAggregation.ApiFilters;
using ApiAggregationService.Features.ApiAggregation.GetAggregatedData;
using ApiAggregationService.Features.ApiAggregation.Services;
using ApiAggregationService.Services.Statistics;

namespace Tests;
public class UnitTests
{
    [Fact]
    public async Task GetAggregatedData_Should_Return_Aggregated_Value()
    {
        // Arrange
        var providers = new List<IApiProvider>
        {
            new FakeProvider("Api1", 100),
            new FakeProvider("Api2", 200),
            new FakeProvider("Api3", 300)
        };

        var service = new ServiceFactory().CreateAggregationService(providers);


        // Act
        var result = await service.GetAggregatedData(
            new AggregatedDataFilter(),
            CancellationToken.None);


        // Assert
        Assert.Equal(600, result.Value.AggregatedValue);
    }

    [Fact]
    public async Task GetAggregatedData_Should_Ignore_Failed_Providers()
    {
        var providers = new List<IApiProvider>
        {
            new FakeProvider("Good", 100),
            new FailingProvider("Bad")
        };


        AggregationService service = new ServiceFactory().CreateAggregationService(providers);

        var result = await service.GetAggregatedData(
            new AggregatedDataFilter(),
            CancellationToken.None);


        Assert.Equal(100, result.Value.AggregatedValue);
        Assert.Equal(1, result.Value.SourcesUsed);
    }

    [Fact]
    public async Task Should_Return_Cached_Data()
    {
        var provider = new FakeProvider("Api", 500);

        AggregationService service = new ServiceFactory().CreateAggregationService(new[] { provider });


        await service.GetAggregatedData(
            new AggregatedDataFilter(),
            CancellationToken.None);


        var result = await service.GetAggregatedData(
            new AggregatedDataFilter(),
            CancellationToken.None);


        Assert.Equal(500, result.Value.AggregatedValue);
        Assert.Equal(1, provider.CallCount);
    }

    [Fact]
    public void Should_Sort_Providers_By_Value()
    {
        var providers = new List<ProviderValue>
        {
            new()
            {
                Provider = "A",
                Value = 100
            },
            new()
            {
                Provider = "B",
                Value = 500
            }
        };


        var result = providers.ApplyFilter(
            new AggregatedDataFilter
            {
                SortBy = AggregatedDataSortBy.Value,
                Direction = SortDirection.Desc
            });


        Assert.Equal("B", result.First().Provider);
    }

    [Fact]
    public void Should_Filter_By_Provider()
    {
        var providers = new List<ProviderValue>
        {
            new()
            {
                Provider = "Github",
                Value=100
            },
            new()
            {
                Provider = "Bitstamp",
                Value=200
            }
        };


        var result = providers.ApplyFilter(
            new AggregatedDataFilter
            {
                SortBy = AggregatedDataSortBy.Provider,
                Direction = SortDirection.Desc
            });


        Assert.Equal("Github", result[0].Provider);
    }

    [Fact]
    public void Should_Record_Fast_Request()
    {
        var statisticsService = new StatisticsService();

        statisticsService.Record("Github", 50, true);


        var stats = statisticsService.GetStatistics().First();


        Assert.Equal(1, stats.FastRequests);
        Assert.Equal(1, stats.TotalRequests);
    }
}