namespace Infrastructure.Models
{
    public sealed class ProductSaleResponse
    {
        public int SaleId { get; init; }

        public int ProductId { get; init; }

        public decimal SalePrice { get; init; }

        public int SaleQty { get; init; }

        public DateOnly SaleDate { get; init; }
    }
}
