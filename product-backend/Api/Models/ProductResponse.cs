namespace Api.Models
{
    public sealed record ProductResponse(
    int Id,
    string Description,
    decimal SalePrice,
    string Category,
    string Image);
}
