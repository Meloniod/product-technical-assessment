using Application.Contracts;
using Infrastructure.Client;
using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddOptions<SalesApiOptions>()
                .Bind(configuration.GetSection(
                    SalesApiOptions.SectionName))
                .Validate(
                    options =>
                        Uri.TryCreate(
                            options.BaseUrl,
                            UriKind.Absolute,
                            out _),
                    "SalesApi:BaseUrl must be a valid absolute URI.")
                .Validate(
                    options => options.TimeoutSeconds is >= 10 and <= 30,
                    "SalesApi:TimeoutSeconds must be between 10 and 30.")
                .ValidateOnStart();

            var timeoutSeconds =
                configuration
                    .GetSection(SalesApiOptions.SectionName)
                    .Get<SalesApiOptions>()?
                    .TimeoutSeconds ?? 20;

            services.AddHttpClient<
                IProductSalesApiClient,
                ProductSalesApiClient>(
                (serviceProvider, client) =>
                {
                    var options =
                        serviceProvider
                            .GetRequiredService<
                                IOptions<SalesApiOptions>>()
                            .Value;

                    client.BaseAddress =
                        new Uri(options.BaseUrl);

                    client.Timeout =
                        Timeout.InfiniteTimeSpan;
                })
                .AddStandardResilienceHandler(options =>
                {
                    options.TotalRequestTimeout.Timeout =
                        TimeSpan.FromSeconds(timeoutSeconds);
                });

            return services;
        }
    }
}
