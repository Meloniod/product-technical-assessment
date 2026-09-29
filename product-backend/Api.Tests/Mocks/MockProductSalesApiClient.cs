using Application.Contracts;
using Domain.Products;
using Domain.Sales;

namespace Api.Tests.Mocks
{
    public sealed class MockProductSalesApiClient
    : IProductSalesApiClient
    {
        public IReadOnlyList<Product> Products { get; set; }
            = Array.Empty<Product>();

        public IReadOnlyList<Sale> Sales { get; set; }
            = Array.Empty<Sale>();

        public Exception? ExceptionToThrow { get; set; }

        public int RequestedProductId { get; private set; }

        public Task<IReadOnlyList<Product>> GetProductsAsync(
            CancellationToken cancellationToken)
        {
            ThrowIfConfigured();

            return Task.FromResult(Products);
        }

        public Task<IReadOnlyList<Sale>> GetProductSalesAsync(
            int productId,
            CancellationToken cancellationToken)
        {
            ThrowIfConfigured();

            RequestedProductId = productId;

            return Task.FromResult(Sales);
        }

        private void ThrowIfConfigured()
        {
            if (ExceptionToThrow is not null)
            {
                throw ExceptionToThrow;
            }
        }
    }
}
