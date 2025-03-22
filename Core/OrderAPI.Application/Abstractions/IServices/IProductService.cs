using OrderAPI.Application.DTOs;

namespace OrderAPI.Application.Abstractions.IServices
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProductsAsync(string? category);
    }
}
