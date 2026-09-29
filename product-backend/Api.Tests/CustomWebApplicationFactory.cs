using Api.Tests.Mocks;
using Application.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Api.Tests
{
    public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
    {
        public MockProductSalesApiClient ApiClient { get; }
            = new();

        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IProductSalesApiClient>();

                services.AddSingleton<IProductSalesApiClient>(
                    ApiClient);
            });
        }
    }
}
