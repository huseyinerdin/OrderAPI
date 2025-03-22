using Microsoft.EntityFrameworkCore;
using OrderAPI.Application.Abstractions.IRepositories;
using OrderAPI.Domain.Entities;
using OrderAPI.Persistence.Data;

namespace OrderAPI.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Where(p => p.Status).ToListAsync();
        }
        public async Task<List<Product>> GetByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Status && p.Category == category)
                .ToListAsync();
        }
    }
}

