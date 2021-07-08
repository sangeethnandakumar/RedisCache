using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCacheAPI
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string key, object response, TimeSpan timeToLive);

        Task<string> GetCachedResponseAsync(string key);
    }
}