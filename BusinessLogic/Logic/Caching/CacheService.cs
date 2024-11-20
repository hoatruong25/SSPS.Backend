using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace BusinessLogic.Logic.Caching
{
    public class CacheService : ICacheService
    {
        private static readonly ConcurrentDictionary<string, bool> CacheKey = new();
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            string? cacheValue = await _distributedCache.GetStringAsync(key);

            if (cacheValue is null) return null;
            
            T? value = JsonConvert.DeserializeObject<T>(cacheValue);

            return value;
        }

        public async Task SetAsync<T>(string key, T value) where T : class
        {
            string cacheValue = JsonConvert.SerializeObject(value);

            await _distributedCache.SetStringAsync(key, cacheValue);

            CacheKey.TryAdd(key, false);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);

            CacheKey.TryRemove(key, out bool _);
        }

        public async Task RemoveByPrefixAsync(string prefixKey)
        {
            //foreach (var key in CacheKey.Keys)
            //{
            //    if (key.StartsWith(prefixKey))
            //    {
            //        await RemoveAsync(key, cancellationToken);
            //    }
            //}

            IEnumerable<Task> tasks = CacheKey
                .Keys
                .Where(k => k.StartsWith(prefixKey))
                .Select(k => RemoveAsync(k));

            await Task.WhenAll(tasks);
        }
    }
}
