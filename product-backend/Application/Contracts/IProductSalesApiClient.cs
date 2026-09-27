using Domain.Products;
using Domain.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
