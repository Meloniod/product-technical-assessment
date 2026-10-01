using Api.Models;
using Domain.Products;
using Domain.Sales;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using Polly.Timeout;

namespace Api.Tests.Controllers
{
    public sealed class ProductsControllerTests
    : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ProductsControllerTests(
            CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsProducts()
        {
            _factory.ApiClient.Products =
                new List<Product>
                {
                new(
                    id: 18,
                    description: "Apricot",
                    salePrice: 16.2m,
                    category: "Fruit",
                    image: "https://example.com/apricot.jpg"),

                new(
                    id: 19,
                    description: "Figs",
                    salePrice: 25m,
                    category: "Fruit",
                    image: "https://example.com/figs.jpg")
                };

            var response =
                await _client.GetAsync(
                    "/api/products");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var products =
                await response.Content
                    .ReadFromJsonAsync<
                        List<ProductResponse>>();

            Assert.NotNull(products);
            Assert.Equal(2, products.Count);

            Assert.Equal(18, products[0].Id);
            Assert.Equal(
                "Apricot",
                products[0].Description);

            Assert.Equal(
                16.2m,
                products[0].SalePrice);
        }

        [Fact]
        public async Task GetProductSalesSummary_ReturnsSummary()
        {
            _factory.ApiClient.Products =
                new List<Product>
                {
                    new(
                        id: 20,
                        description: "Cherries",
                        salePrice: 16.2m,
                        category: "Fruit",
                        image: "https://example.com/cherries.jpg")
                };

            _factory.ApiClient.Sales =
                new List<Sale>
                {
            new(
                saleId: 2028,
                productId: 20,
                salePrice: 355.52m,
                saleQuantity: 6760,
                saleDate: new DateOnly(
                    2026,
                    9,
                    20)),

            new(
                saleId: 2029,
                productId: 20,
                salePrice: 425.60m,
                saleQuantity: 3600,
                saleDate: new DateOnly(
                    2026,
                    9,
                    21)),

            new(
                saleId: 2030,
                productId: 20,
                salePrice: 416.00m,
                saleQuantity: 6040,
                saleDate: new DateOnly(
                    2026,
                    9,
                    22))
                };

            var response =
                await _client.GetAsync(
                    "/api/products/20/sales-summary");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var summary =
                await response.Content
                    .ReadFromJsonAsync<
                        ProductSalesSummaryResponse>();

            Assert.NotNull(summary);

            Assert.Equal(20, summary.ProductId);
            Assert.Equal(3, summary.NumberOfSales);
            Assert.Equal(16400, summary.TotalQuantity);
            Assert.Equal(1197.12m, summary.TotalSales);

            Assert.Equal(
                20,
                _factory.ApiClient.RequestedProductId);
        }

        [Fact]
        public async Task GetProductSalesSummary_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
        {
            _factory.ApiClient.Products =
                new List<Product>
                {
                    new(
                        id: 20,
                        description: "Cherries",
                        salePrice: 16.2m,
                        category: "Fruit",
                        image: "https://example.com/cherries.jpg")
                };

            var response =
                await _client.GetAsync(
                    "/api/products/999/sales-summary");

            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);

            var problem =
                await response.Content
                    .ReadFromJsonAsync<ProblemDetails>();

            Assert.NotNull(problem);
            Assert.Equal(404, problem.Status);
            Assert.Equal("Product not found.", problem.Title);
            Assert.Equal(
                "Product with ID 999 was not found.",
                problem.Detail);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task GetProductSalesSummary_WithInvalidId_ReturnsBadRequest(
        int productId)
        {
            var response =
                await _client.GetAsync(
                    $"/api/products/{productId}/sales-summary");

            Assert.Equal(
                HttpStatusCode.BadRequest,
                response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_WhenExternalApiFails_ReturnsBadGateway()
        {
            _factory.ApiClient.ExceptionToThrow =
                new HttpRequestException(
                    "External API unavailable.");

            try
            {
                var response =
                    await _client.GetAsync(
                        "/api/products");

                Assert.Equal(
                    HttpStatusCode.BadGateway,
                    response.StatusCode);
            }
            finally
            {
                _factory.ApiClient.ExceptionToThrow = null;
            }
        }

        [Fact]
        public async Task GetProducts_WhenExternalApiTimesOut_ReturnsGatewayTimeout()
        {
            _factory.ApiClient.ExceptionToThrow =
                new TimeoutRejectedException(
                    "The external API timed out.");

            try
            {
                var response =
                    await _client.GetAsync(
                        "/api/products");

                Assert.Equal(
                    HttpStatusCode.GatewayTimeout,
                    response.StatusCode);
            }
            finally
            {
                _factory.ApiClient.ExceptionToThrow = null;
            }
        }
    }
}
