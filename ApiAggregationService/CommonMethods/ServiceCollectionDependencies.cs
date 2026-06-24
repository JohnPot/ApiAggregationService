using ApiAggregationService.ExternalApis.Providers;
using ApiAggregationService.ExternalApis.Providers.Bitstamp;
using ApiAggregationService.ExternalApis.Providers.Github;
using ApiAggregationService.ExternalApis.Providers.OpenWeatherMapProvider;
using ApiAggregationService.ExternalApis.Providers.PokeProvider;
using ApiAggregationService.BackgroundWorkers;
using ApiAggregationService.ExternalApis;
using ApiAggregationService.Features.ApiAggregation.Services.Statistics;
using ApiAggregationService.Features.ApiAggregation.Services.ValueTransformation;
using ApiAggregationService.Features.ApiAggregation.Services.Aggregation;

namespace ApiAggregationService.CommonMethods;

public static class ServiceCollectionDependencies
{
    public static IServiceCollection AddDependencyServices(this IServiceCollection services)
    {
        services.AddScoped<IAggregationService, AggregationService>();
        services.AddScoped<IValueTransformation, ValueTransformationService>();
        services.AddSingleton<IStatisticsService, StatisticsService>();

        return services;
    }

    public static IServiceCollection AddDependencyProviders(this IServiceCollection services)
    {
        services.AddScoped<IApiProvider>(sp => sp.GetRequiredService<BitfinexProvider>());
        services.AddScoped<IApiProvider>(sp => sp.GetRequiredService<BitstampProvider>());
        services.AddScoped<IApiProvider>(sp => sp.GetRequiredService<PokeProvider>());
        services.AddScoped<IApiProvider>(sp => sp.GetRequiredService<OpenWeatherMapProvider>());
        services.AddScoped<IApiProvider>(sp => sp.GetRequiredService<GithubProvider>());

        return services;
    }

    public static IServiceCollection AddDependencyHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<BitstampProvider>(client =>
        {
            client.BaseAddress = new Uri("https://www.bitstamp.net/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        services.AddHttpClient<BitfinexProvider>(client =>
        {
            client.BaseAddress = new Uri("https://api-pub.bitfinex.com/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        services.AddHttpClient<OpenWeatherMapProvider>(client =>
        {
            client.BaseAddress = new Uri("https://api.openweathermap.org/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        services.AddHttpClient<PokeProvider>(client =>
        {
            client.BaseAddress = new Uri("https://pokeapi.co/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        services.AddHttpClient<GithubProvider>(client =>
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("ApiAggregationService");
        });

        return services;
    }

    public static IServiceCollection AddDependencyHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<ServiceAnalyzer>();

        return services;
    }
}
