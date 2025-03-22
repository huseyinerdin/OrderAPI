using OrderAPI.Domain.Entities;

namespace OrderAPI.Application.Abstractions.IRepositories
{
    public interface IOrderRepository
    {
        Task CreateAsync(Order order);
        Task SaveChangesAsync();
    }
}
