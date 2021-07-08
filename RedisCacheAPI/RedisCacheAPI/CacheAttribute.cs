using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLive;

        public CacheAttribute(int timeToLive)
        {
            this.timeToLive = timeToLive;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var settings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();
            if (settings.Enable)
            {
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
                var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
                var cacheResponse = await cacheService.GetCachedResponseAsync(cacheKey);
                if (!string.IsNullOrEmpty(cacheResponse))
                {
                    var contentResult = new ContentResult
                    {
                        Content = cacheResponse,
                        ContentType = "application/json",
                        StatusCode = 200
                    };
                    context.Result = contentResult;
                    return;
                }
                var executeContext = await next();
                if (executeContext.Result is OkObjectResult objectResult)
                {
                    await cacheService.CacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(timeToLive));
                }
            }
            else
            {
                await next();
            }
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}