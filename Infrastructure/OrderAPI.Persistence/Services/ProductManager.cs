using AutoMapper;
using OrderAPI.Application.Abstractions.IRepositories;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Application.DTOs;
using OrderAPI.Domain.Entities;

namespace OrderAPI.Persistence.Services
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICacheService _cache;
        private readonly IMapper _mapper;

        public ProductManager(IProductRepository repo, IMapper mapper, ICacheService cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<ProductDTO>> GetProductsAsync(string? category)
        {
            string cacheKey = string.IsNullOrEmpty(category) ? "all_products" : $"products_{category}";

            var cached = await _cache.GetAsync<List<ProductDTO>>(cacheKey);
            if (cached != null)
                return cached;

            List<Product> products = string.IsNullOrEmpty(category)
                ? await _repo.GetAllAsync()
                : await _repo.GetByCategoryAsync(category);

            var dtoList = _mapper.Map<List<ProductDTO>>(products);

            await _cache.SetAsync(cacheKey, dtoList, TimeSpan.FromMinutes(5));

            return dtoList;
        }
    }
}
