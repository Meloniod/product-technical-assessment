using Domain.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Helpers
{
    public sealed class SalesSummaryCalculator
    {
        public ProductSalesSummary Calculate(
            int productId,
            IReadOnlyCollection<Sale> sales)
        {
            if (productId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productId));
            }

            ArgumentNullException.ThrowIfNull(sales);

            var numberOfSales = sales.Count;

            var totalQuantity = sales.Sum(
                sale => sale.SaleQuantity);

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
