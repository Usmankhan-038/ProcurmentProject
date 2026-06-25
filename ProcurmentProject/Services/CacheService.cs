using Microsoft.Extensions.Caching.Memory;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;

namespace ProcurmentProject.Services
{
    public class CacheService :ICacheService
    {
        private readonly IMemoryCache _cache;
        private TimeSpan _defaultExpiration  = TimeSpan.FromMinutes(60);
        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<T> GetOrSetCache<T>(string cacheKey, Func<Task<T>> cacheFunction, TimeSpan? expiration = null)
        {
            if (!_cache.TryGetValue(cacheKey, out T result))
            {
                result = await cacheFunction();
                _cache.Set(cacheKey, result, expiration ?? _defaultExpiration);
            }
            return result;
        }

        public void RemoveCache(string key) => _cache.Remove(key);
        public T? GetCache<T>(string key)
        {
            if(!_cache.TryGetValue(key, out T result))
            {
                return default;
                
            }
            return result;
        }
    }
}
