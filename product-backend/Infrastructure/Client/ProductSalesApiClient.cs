using Application.Contracts;
using Domain.Products;
using Domain.Sales;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Infrastructure.Client
{
    public sealed class ProductSalesApiClient : IProductSalesApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductSalesApiClient> _logger;

        public ProductSalesApiClient(
            HttpClient httpClient,
            ILogger<ProductSalesApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Requesting products from external sales API.");

            var products =
                await _httpClient.GetFromJsonAsync<
                    IReadOnlyList<ProductResponse>>(
                        "products",
                        cancellationToken);

            if (products is null)
            {
                return Array.Empty<Product>();
            }

            return products
                .Select(MapProduct)
                .ToList();
        }

        public async Task<IReadOnlyList<Sale>> GetProductSalesAsync(
            int productId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Requesting sales for product {ProductId}.",
                productId);

            var sales =
                await _httpClient.GetFromJsonAsync<
                    IReadOnlyList<ProductSaleResponse>>(
                        $"product-sales?Id={productId}",
                        cancellationToken);

            if (sales is null)
            {
                return Array.Empty<Sale>();
            }

            return sales
                .Select(MapSale)
                .ToList();
        }

        private static Product MapProduct(
            ProductResponse response)
        {
            return new Product(
                response.Id,
                response.Description,
                response.SalePrice,
                response.Category,
                response.Image);
        }

        private static Sale MapSale(
            ProductSaleResponse response)
        {
            return new Sale(
                response.SaleId,
                response.ProductId,
                response.SalePrice,
                response.SaleQty,
                response.SaleDate);
        }
    }
}
