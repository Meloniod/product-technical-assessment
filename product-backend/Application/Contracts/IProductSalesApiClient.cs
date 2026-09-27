using Domain.Products;
using Domain.Sales;

namespace Application.Contracts
{
    public interface IProductSalesApiClient
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(
            CancellationToken cancellationToken);

        Task<IReadOnlyList<Sale>> GetProductSalesAsync(
            int productId,
            CancellationToken cancellationToken);
    }
}
