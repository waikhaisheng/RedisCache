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
using StackExchange.Redis;

namespace NUnitTestProjectRedis.TestWebApplicationRedis.Extensions
{
    public class TestRedisCacheExtension
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
                var multiplexer = ConnectionMultiplexer.Connect("localhost");
                services.AddSingleton<IConnectionMultiplexer>(multiplexer);
                serviceProvider = services.BuildServiceProvider();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Test]
        public void Test_SetData()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var rng = new Random();
            var rid = "id1";

            var data = Enumerable.Range(1, 5).Select(index => rng.Next(-20, 55).ToString())
                        .ToArray();

            srv.SetData<string[]>(rid, data);

            Assert.Pass();
        }

        [Test]
        public void Test_GetData()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var rid = "id1";
            var data = srv.GetData<string[]>(rid);

            Assert.IsNotNull(data);
        }
    }
}
