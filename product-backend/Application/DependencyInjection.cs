using Application.Services;
using Application.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
