using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Sales
{
    public sealed class ProductSalesSummary
    {
        public int ProductId { get; }
        public int NumberOfSales { get; }
        public int TotalQuantity { get; }
        public decimal TotalSales { get; }

        public ProductSalesSummary(
            int productId,
            int numberOfSales,
            int totalQuantity,
            decimal totalSales)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId));

            if (numberOfSales < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfSales));

            if (totalQuantity < 0)
                throw new ArgumentOutOfRangeException(nameof(totalQuantity));

            if (totalSales < 0)
                throw new ArgumentOutOfRangeException(nameof(totalSales));

            ProductId = productId;
            NumberOfSales = numberOfSales;
            TotalQuantity = totalQuantity;
            TotalSales = totalSales;
        }
    }
}
