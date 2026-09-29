using Domain.Sales;

namespace Application.Services.Helpers
{
    public sealed class SalesSummaryCalculator
    {
        public ProductSalesSummary Calculate(
            int productId,
            IReadOnlyCollection<Sale> sales)
        {
            ArgumentNullException.ThrowIfNull(sales);

            var numberOfSales = sales.Count;

            var totalQuantity = sales.Sum(
                sale => sale.SaleQuantity);

            // The assessment API exposes salePrice but does not document whether
            // it represents a unit price or the monetary value of the sale record.
            // The current implementation treats it as the monetary value supplied
            // by each sale record and therefore sums salePrice directly.
            var totalSales = sales.Sum(
                sale => sale.SalePrice);

            return new ProductSalesSummary(
                productId,
                numberOfSales,
                totalQuantity,
                totalSales);
        }
    }
}
