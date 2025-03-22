using Microsoft.AspNetCore.Mvc;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Application.DTOs;
using OrderAPI.Application.Enums;

namespace OrderAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var orderId = await _orderService.CreateOrderAsync(request);
                return Ok(new ApiResponse<int>
                {
                    Status = ResponseStatus.Success,
                    ResultMessage = "Order was created successfully.",
                    Data = orderId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int>
                {
                    Status = ResponseStatus.Failed,
                    ResultMessage = "An error occurred while creating the order.",
                    ErrorCode = "ORDER_CREATE_ERROR",
                    Data = 0
                });
            }
        }
    }
}
