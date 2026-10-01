using Application.Contracts;
using Application.Services.Helpers;
using Domain.Products;
using Domain.Sales;

namespace Application.Services
{
    public sealed class ProductSalesService
    {
        private readonly IProductSalesApiClient _apiClient;
        private readonly SalesSummaryCalculator _summaryCalculator;

        public ProductSalesService(
            IProductSalesApiClient apiClient,
            SalesSummaryCalculator summaryCalculator)
        {
            _apiClient = apiClient;
            _summaryCalculator = summaryCalculator;
        }

        public Task<IReadOnlyList<Product>> GetProductsAsync(
            CancellationToken cancellationToken)
        {
            return _apiClient.GetProductsAsync(cancellationToken);
        }

        public async Task<ProductSalesSummary> GetProductSalesSummaryAsync(
            int productId,
            CancellationToken cancellationToken)
        {
            if (productId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(productId),
                    "Product ID must be greater than zero.");
            }

            var products = await _apiClient.GetProductsAsync(
                cancellationToken);

            if (!products.Any(product => product.Id == productId))
            {
                throw new KeyNotFoundException(
                    $"Product with ID {productId} was not found.");
            }

            var sales = await _apiClient.GetProductSalesAsync(
                productId,
                cancellationToken);

            return _summaryCalculator.Calculate(
                productId,
                sales);
        }
    }
}
