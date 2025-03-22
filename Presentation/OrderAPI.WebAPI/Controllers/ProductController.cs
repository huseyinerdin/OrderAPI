using Microsoft.AspNetCore.Mvc;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Application.DTOs;
using OrderAPI.Application.Enums;

namespace OrderAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductDTO>>>> GetProducts([FromQuery] string? category)
        {
            try
            {
                var data = await _productService.GetProductsAsync(category);
                return Ok(new ApiResponse<List<ProductDTO>>
                {
                    Status = ResponseStatus.Success,
                    ResultMessage = "Products listed.",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<ProductDTO>>
                {
                    Status = ResponseStatus.Failed,
                    ResultMessage = "An error occured.",
                    ErrorCode = "PRODUCT_LIST_ERROR",
                    Data = null
                });
            }
        }
    }
}
