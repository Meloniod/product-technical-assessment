using Application.Contracts;
using Domain.Products;
using Domain.Sales;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Client
{
    public sealed class ProductSalesApiClient : IProductSalesApiClient
    {
        private readonly HttpClient _httpClient;

        public ProductSalesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(
            CancellationToken cancellationToken)
        {
            var products = await _httpClient.GetFromJsonAsync<
                List<ProductResponse>>(
                    "products",
                    cancellationToken);

            if (products is null)
            {
                return [];
            }

            return products
                .Select(MapProduct)
                .ToList();
        }

        public async Task<IReadOnlyList<Sale>> GetProductSalesAsync(
            int productId,
            CancellationToken cancellationToken)
        {
            var sales = await _httpClient.GetFromJsonAsync<
                List<ProductSaleResponse>>(
                    $"product-sales?Id={productId}",
                    cancellationToken);

            if (sales is null)
            {
                return [];
            }

            return sales
                .Select(MapSale)
                .ToList();
        }

        private static Product MapProduct(ProductResponse response)
        {
            return new Product(
                response.Id,
                response.Description,
                response.SalePrice,
                response.Category,
                response.Image);
        }

        private static Sale MapSale(ProductSaleResponse response)
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
