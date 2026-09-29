namespace Api.Models
{
    public sealed record ProductSalesSummaryResponse(
    int ProductId,
    int NumberOfSales,
    int TotalQuantity,
    decimal TotalSales);
}
