using Application.Services;
using Application.Services.Helpers;
using Application.Tests.Mocks;
using Domain.Sales;

namespace Application.Tests.Services
{
    public sealed class ProductSalesServiceTests
    {
        [Fact]
        public async Task GivenProductQuery_WhenGettingProductSalesSummaryAsync_ThenRequestFindsSelectedProduct()
        {
            // Arrange
            var apiClient = new MockProductSalesApiClient
            {
                Sales = new List<Sale>
            {
                new(
                    saleId: 1,
                    productId: 20,
                    salePrice: 100m,
                    saleQuantity: 10,
                    saleDate: new DateOnly(2026, 9, 20))
            }
            };

            var calculator = new SalesSummaryCalculator();

            var service = new ProductSalesService(
                apiClient,
                calculator);

            // Act
            var result =
                await service.GetProductSalesSummaryAsync(
                    20,
                    CancellationToken.None);

            // Assert
            Assert.Equal(20, apiClient.RequestedProductId);
            Assert.Equal(1, result.NumberOfSales);
            Assert.Equal(10, result.TotalQuantity);
            Assert.Equal(100m, result.TotalSales);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GivenSalesSummaryAsync_WhenProductIdisInvalidId_ThenMethodThrows(int productId)
        {
            // Arrange
            var apiClient = new MockProductSalesApiClient();

            var calculator = new SalesSummaryCalculator();

            var service = new ProductSalesService(
                apiClient,
                calculator);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () =>
                    service.GetProductSalesSummaryAsync(
                        productId,
                        CancellationToken.None));
        }

        [Fact]
        public async Task GivenRequestProductSalesSummary_WhenCancellationTokenSentInRequest_ThenPropagateCancellationToken()
        {
            // Arrange
            var apiClient = new MockProductSalesApiClient();

            var calculator = new SalesSummaryCalculator();

            var service = new ProductSalesService(
                apiClient,
                calculator);

            using var cancellationTokenSource =
                new CancellationTokenSource();

            var cancellationToken =
                cancellationTokenSource.Token;

            // Act
            await service.GetProductSalesSummaryAsync(
                20,
                cancellationToken);

            // Assert
            Assert.Equal(
                cancellationToken,
                apiClient.ReceivedCancellationToken);
        }
    }
}
