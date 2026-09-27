using Application.Contracts;
using Application.Services.Helpers;
using Domain.Products;
using Domain.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var sales = await _apiClient.GetProductSalesAsync(
                productId,
                cancellationToken);

            return _summaryCalculator.Calculate(
                productId,
                sales);
        }
    }
}
