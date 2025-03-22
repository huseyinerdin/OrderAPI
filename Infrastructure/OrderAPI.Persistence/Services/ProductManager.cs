using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using OrderAPI.Application.Abstractions.IRepositories;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Application.DTOs;
using OrderAPI.Domain.Entities;

namespace OrderAPI.Persistence.Services
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductManager(IProductRepository repo, IMapper mapper, IMemoryCache cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<ProductDTO>> GetProductsAsync(string? category)
        {
            string cacheKey = string.IsNullOrEmpty(category) ? "all_products" : $"products_{category}";

            if (_cache.TryGetValue(cacheKey, out List<ProductDTO> cached))
            {
                return cached;
            }

            List<Product> products = string.IsNullOrEmpty(category)
                ? await _repo.GetAllAsync()
                : await _repo.GetByCategoryAsync(category);

            var dtoList = _mapper.Map<List<ProductDTO>>(products);

            _cache.Set(cacheKey, dtoList, TimeSpan.FromMinutes(5));

            return dtoList;
        }
    }
}
