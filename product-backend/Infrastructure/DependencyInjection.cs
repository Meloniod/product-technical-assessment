using Application.Contracts;
using Infrastructure.Client;
using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.Configure<SalesApiOptions>(
                configuration.GetSection(
                    SalesApiOptions.SectionName));

            services.AddHttpClient<IProductSalesApiClient,
                ProductSalesApiClient>((serviceProvider, client) =>
                {
                    var options =
                        serviceProvider
                            .GetRequiredService<
                                Microsoft.Extensions.Options
                                    .IOptions<SalesApiOptions>>()
                            .Value;

                    if (string.IsNullOrWhiteSpace(
                        options.BaseUrl))
                    {
                        throw new InvalidOperationException(
                            "SalesApi:BaseUrl is not configured.");
                    }

                    client.BaseAddress =
                        new Uri(options.BaseUrl);

                    client.Timeout =
                        TimeSpan.FromSeconds(
                            options.TimeoutSeconds);
                });

            return services;
        }
    }
}
