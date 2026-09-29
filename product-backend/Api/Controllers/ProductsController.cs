using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly ProductSalesService _productSalesService;

        public ProductsController(ProductSalesService productSalesService)
        {
            _productSalesService = productSalesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
            CancellationToken cancellationToken)
        {
            var products = await _productSalesService.GetProductsAsync(
                cancellationToken);

            return Ok(products);
        }

        [HttpGet("{productId:int}/sales-summary")]
        public async Task<IActionResult> GetSalesSummary(
            int productId,
            CancellationToken cancellationToken)
        {
            var summary =
                await _productSalesService.GetProductSalesSummaryAsync(
                    productId,
                    cancellationToken);

            return Ok(summary);
        }
    }
}
