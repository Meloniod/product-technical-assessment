using Application.Contracts;
using Application.Services;
using Application.Services.Helpers;
using Domain.Products;
using Domain.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Mocks
{
    public sealed class MockProductSalesApiClient
    : IProductSalesApiClient
    {
        public IReadOnlyList<Product> Products { get; set; } =
            Array.Empty<Product>();

        public IReadOnlyList<Sale> Sales { get; set; } =
            Array.Empty<Sale>();

        public int RequestedProductId { get; private set; }

        public CancellationToken ReceivedCancellationToken { get; private set; }

        public Task<IReadOnlyList<Product>> GetProductsAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Products);
        }

        public Task<IReadOnlyList<Sale>> GetProductSalesAsync(
            int productId,
            CancellationToken cancellationToken)
        {
            RequestedProductId = productId;
            ReceivedCancellationToken = cancellationToken;

            return Task.FromResult(Sales);
        }

        
    }
}
