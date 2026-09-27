using Application.Contracts;
using Infrastructure.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        private const string BaseUrl =
            "https://singularsystems-tech-assessment-sales-api2.azurewebsites.net/";

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddHttpClient<IProductSalesApiClient, ProductSalesApiClient>(
                client =>
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(300);
                });

            return services;
        }
    }
}
