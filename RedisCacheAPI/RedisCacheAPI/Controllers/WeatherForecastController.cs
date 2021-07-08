using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCacheAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ICacheService cacheService;
        private static RedisCacheSettings redis;

        public WeatherForecastController(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string key)
        {
            var data = await cacheService.GetCacheValueAsync(key);
            return Ok(data);
        }

        //This methord is a response cached methord using Redis
        [HttpGet]
        [Route("GetList")]
        [Cache(5)]
        public async Task<IActionResult> GetList([FromQuery] string key)
        {
            var data = RunScript();
            return Ok(data);
        }

        private List<double> RunScript()
        {
            var rand = new Random();
            var rtnlist = new List<double>();

            for (int i = 0; i < 100; i++)
            {
                rtnlist.Add(rand.Next(1000));
            }
            return rtnlist;
        }

        [HttpPost]
        public IActionResult Set([FromBody] dynamic data)
        {
            cacheService.SetCacheValueAsync("car", "Tesla");
            return Ok("Done");
        }
    }
}