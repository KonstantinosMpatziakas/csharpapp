using Polly.Extensions.Http;
using Polly;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        services.Configure<RestApiSettings>(configuration!.GetSection(nameof(RestApiSettings)));
        services.Configure<HttpClientSettings>(configuration.GetSection(nameof(HttpClientSettings)));

        services.AddSingleton<IProductsService, ProductsService>();

        services.AddHttpClient("productsApi", client =>
        {
            client.BaseAddress = new Uri(configuration!.GetSection(nameof(RestApiSettings)).GetSection("BaseUrl").Value);
        });

        return services;
    }
}