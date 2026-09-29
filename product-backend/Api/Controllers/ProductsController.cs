using Api.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly ProductSalesService _productSalesService;

        public ProductsController(
            ProductSalesService productSalesService)
        {
            _productSalesService = productSalesService;
        }

        [HttpGet]
        [ProducesResponseType(
    StatusCodes.Status200OK)]
        public async Task<ActionResult<
    IReadOnlyList<ProductResponse>>> GetProducts(
    CancellationToken cancellationToken)
        {
            var products =
                await _productSalesService.GetProductsAsync(
                    cancellationToken);

            var response =
                products
                    .Select(product =>
                        new ProductResponse(
                            product.Id,
                            product.Description,
                            product.SalePrice,
                            product.Category,
                            product.Image))
                    .ToList();

            return Ok(response);
        }

        [HttpGet("{productId:int}/sales-summary")]
        [ProducesResponseType(
        StatusCodes.Status200OK)]
            [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<
        ProductSalesSummaryResponse>>
        GetProductSalesSummary(
        int productId,
        CancellationToken cancellationToken)
        {
            var summary =
                await _productSalesService
                    .GetProductSalesSummaryAsync(
                        productId,
                        cancellationToken);

            var response =
                new ProductSalesSummaryResponse(
                    summary.ProductId,
                    summary.NumberOfSales,
                    summary.TotalQuantity,
                    summary.TotalSales);

            return Ok(response);
        }
    }
}
