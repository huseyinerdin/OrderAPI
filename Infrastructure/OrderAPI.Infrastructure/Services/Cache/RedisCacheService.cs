using Microsoft.Extensions.Configuration;
using OrderAPI.Application.Abstractions.IServices;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderAPI.Infrastructure.Services.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _cache;

        public RedisCacheService(IConfiguration configuration)
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]);
            _cache = redis.GetDatabase();
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _cache.StringSetAsync(key, json, expiry);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public bool KeyExists(string key)
        {
            return _cache.KeyExists(key);
        }
    }
}
