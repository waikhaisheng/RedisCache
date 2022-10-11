using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using WebApplicationRedis.Extensions;

namespace NUnitTestProjectRedis.TestWebApplicationRedis.Extensions
{
    public class DistributedCacheExtensions
    {
        private IConfiguration Configuration;
        private ServiceProvider serviceProvider;
        
        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.webapplicationredis.test.json")
                .Build();
            return config;
        }
        
        [SetUp]
        public void Setup()
        {
            try
            {
                Configuration = InitConfiguration();
                var services = new ServiceCollection();
                services.AddStackExchangeRedisCache(option =>
                {
                    option.Configuration = Configuration.GetConnectionString("Redis");
                    option.InstanceName = "RedisDemo_";
                });
                serviceProvider = services.BuildServiceProvider();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Test]
        public void Test_SetGetRecordAsync()
        {
            var srv = serviceProvider.GetRequiredService<IDistributedCache>();

            var rng = new Random();
            var rid = "id1";

            var data = Enumerable.Range(1, 5).Select(index => rng.Next(-20, 55).ToString())
                        .ToArray();

            srv.SetRecordAsync<string[]>(rid, data).GetAwaiter().GetResult();
            data = srv.GetRecordAsync<string[]>(rid).GetAwaiter().GetResult();

            Assert.IsNotNull(data);
        }
    }
}
