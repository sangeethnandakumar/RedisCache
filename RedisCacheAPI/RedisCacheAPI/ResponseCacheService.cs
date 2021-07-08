using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisCacheAPI
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response == null)
            {
                return;
            }
            var serialiseResponse = JsonSerializer.Serialize(response);
            await distributedCache.SetStringAsync(key, serialiseResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await distributedCache.GetStringAsync(key);
            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}