using Application.Services;
using Application.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<ProductSalesService>();
            services.AddScoped<SalesSummaryCalculator>();

            return services;
        }
    }
}
