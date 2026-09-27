using Application.Services.Helpers;
using Domain.Sales;

namespace Application.Tests.Services
{
    public sealed class SalesSummaryCalculatorTests
    {
        [Fact]
        public void GivenSalesData_WhenCalculatingSales_ThenReturnCorrectSummary()
        {
            // Arrange
            var calculator = new SalesSummaryCalculator();

            var sales = new List<Sale>
            {
                new(
                    saleId: 2028,
                    productId: 20,
                    salePrice: 355.52m,
                    saleQuantity: 6760,
                    saleDate: new DateOnly(2026, 9, 20)),

                new(
                    saleId: 2029,
                    productId: 20,
                    salePrice: 425.60m,
                    saleQuantity: 3600,
                    saleDate: new DateOnly(2026, 9, 21)),

                new(
                    saleId: 2030,
                    productId: 20,
                    salePrice: 416.00m,
                    saleQuantity: 6040,
                    saleDate: new DateOnly(2026, 9, 22))
            };

            // Act
            var result = calculator.Calculate(
                productId: 20,
                sales);

            // Assert
            Assert.Equal(20, result.ProductId);
            Assert.Equal(3, result.NumberOfSales);
            Assert.Equal(16400, result.TotalQuantity);
            Assert.Equal(1197.12m, result.TotalSales);
        }

        public void GivenNoSalesData_WhenCalculatingWithNoSales_ThenReturnEmptySummary()
        {
            // Arrange
            var calculator = new SalesSummaryCalculator();

            var sales = Array.Empty<Sale>();

            // Act
            var result = calculator.Calculate(
                productId: 20,
                sales);

            // Assert
            Assert.Equal(20, result.ProductId);
            Assert.Equal(0, result.NumberOfSales);
            Assert.Equal(0, result.TotalQuantity);
            Assert.Equal(0m, result.TotalSales);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void GivenProductLookup_WhenInvalidProductIdUsed_ThenMethodThrows(int productId)
        {
            // Arrange
            var calculator = new SalesSummaryCalculator();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => calculator.Calculate(
                    productId,
                    Array.Empty<Sale>()));
        }
    }
}
