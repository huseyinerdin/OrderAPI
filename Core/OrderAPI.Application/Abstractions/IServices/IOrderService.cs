using OrderAPI.Application.DTOs;

namespace OrderAPI.Application.Abstractions.IServices
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderRequest request);
    }
}
