using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationRedis.Extensions;

namespace WebApplicationRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _redis;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IDistributedCache distributedCache,
            IConnectionMultiplexer redis)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _redis = redis;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var rid = "id1";
            
            var data = await _distributedCache.GetRecordAsync<WeatherForecast[]>(rid);
            
            if (data == null)
            {
                data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
                
                await _distributedCache.SetRecordAsync<WeatherForecast[]>(rid, data);
            }

            return data;
        }

        [HttpGet("foo")]
        public async Task<IActionResult> Foo()
        {
            var db = _redis.GetDatabase(1);
            db.StringSet("foo", "Test123");
            var foo = await db.StringGetAsync("foo");
            return Ok(foo.ToString());
        }
    }
}
