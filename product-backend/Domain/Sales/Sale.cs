using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Sales
{
    public sealed class Sale
    {
        public int SaleId { get; }
        public int ProductId { get; }
        public decimal SalePrice { get; }
        public int SaleQuantity { get; }
        public DateOnly SaleDate { get; }

        public Sale(
            int saleId,
            int productId,
            decimal salePrice,
            int saleQuantity,
            DateOnly saleDate)
        {
            SaleId = saleId;
            ProductId = productId;
            SalePrice = salePrice;
            SaleQuantity = saleQuantity;
            SaleDate = saleDate;
        }
    }
}
