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
                var cs = Configuration.GetConnectionString("Redis");
                var multiplexer = ConnectionMultiplexer.Connect(cs);
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
            Test_GetData();

            Assert.Pass();
        }

        //[Test]
        public void Test_GetData()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var rid = "id1";
            var data = srv.GetData<string[]>(rid);

            Assert.IsNotNull(data);
        }

        [Test]
        public void Test_SetDataNull()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var rid = "id1";
            srv.SetData<string[]>(rid, null);
            var data = srv.GetData<string[]>(rid);

            Assert.AreEqual(null, data);
        }

        [Test]
        public void Test_SetDataNullInt()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var rid = "id1";
            srv.SetData<string[]>(rid, null);
            var data = srv.GetData<int>(rid);

            Assert.AreEqual(0, data);
        }
        
        [Test]
        public void Test_GetDataNull()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            string rid = null;
            var data = srv.GetData<string[]>(rid);

            Assert.AreEqual(null, data);
        }

        [Test]
        public void Test_DeleteKey()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var rng = new Random();
            var db = 1;
            var rid = "id1";
            var data = Enumerable.Range(1, 5).Select(index => rng.Next(-20, 55).ToString())
            .ToArray();

            srv.SetData<string[]>(rid, data, db);

            var ret = srv.DeleteKey(db, rid);

            Assert.IsTrue(ret);
        }
        
        [Test]
        public void Test_DeleteKey0()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var db = 1;
            var rid = new string[] { };
            var ret = srv.DeleteKey(db, rid);

            Assert.IsTrue(!ret);
        }
        
        [Test]
        public void Test_DeleteKeyWithNull()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var db = 1;
            var rid = new string[] { null };
            var ret = srv.DeleteKey(db, rid);

            Assert.IsTrue(!ret);
        }
        
        [Test]
        public void Test_DeleteKeyNull()
        {
            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            var db = 1;
            var ret = srv.DeleteKey(db, null);

            Assert.IsTrue(!ret);
        }
        
        [Test]
        public void Test_()
        {


            Assert.Pass();
        }
    }
}
