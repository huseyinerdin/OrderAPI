using OrderAPI.Domain.Entities;

namespace OrderAPI.Application.Abstractions.IRepositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetByCategoryAsync(string category);
    }
}
