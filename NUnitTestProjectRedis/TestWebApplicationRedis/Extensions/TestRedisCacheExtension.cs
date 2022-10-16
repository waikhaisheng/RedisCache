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
using Newtonsoft.Json;
using System.Threading;
using WebApplicationRedis.Models.RedisModels;

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
        public void Test_SetAdd()
        {

            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            IDatabase db = srv.GetDatabase(0);
            db.SetAdd("key1", "v1");
            Assert.Pass();
        }

        [Test]
        public void Test_HashSet()
        {

            var srv = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            IDatabase db = srv.GetDatabase(0);
            db.HashSet("user:user1", new HashEntry[] { new HashEntry("12", "13"), new HashEntry("14", "15") });
            Assert.Pass();
        }

        [Test]
        public void Test_RedisStringData()
        {
            try
            {
                //store
                var l = new List<StudentName>();
                l.Add(new StudentName { Id = 1, Name = "one" });
                l.Add(new StudentName { Id = 2, Name = "two" });
                //convert to redis type
                var lr = new RedisStringData();
                lr.DataType = l.GetType().ToString();
                lr.Data = JsonConvert.SerializeObject(l);
                var rs = JsonConvert.SerializeObject(lr);

                //get
                var redisJsonData = JsonConvert.DeserializeObject<RedisStringData>(rs);
                var dataType = Type.GetType(redisJsonData.DataType);

                var x = JsonConvert.DeserializeObject(redisJsonData.Data, dataType);
                var dataConvert = Convert.ChangeType(x, dataType);//https://referencesource.microsoft.com/#mscorlib/system/convert.cs,441ea31c17007e78

                Assert.AreEqual(x.GetType(), l.GetType());

            }
            catch (Exception ex)
            {

                throw;
            }
            Assert.Pass();
        }

        [Test]
        public void Test_RedisObjectData()
        {
            try
            {
                //store
                var l = new List<StudentName>();
                l.Add(new StudentName { Id = 1, Name = "one" });
                l.Add(new StudentName { Id = 2, Name = "two" });
                //convert to redis type
                var lr = new RedisObjectData();
                lr.DataType = l.GetType().ToString();
                lr.Data = l;
                var rs = JsonConvert.SerializeObject(lr);

                //get
                var redisJsonData = JsonConvert.DeserializeObject<RedisObjectData>(rs);
                var dataType = Type.GetType(redisJsonData.DataType);
                var dataConvert = Convert.ChangeType(redisJsonData.Data, dataType);//https://referencesource.microsoft.com/#mscorlib/system/convert.cs,441ea31c17007e78 //Object must implement IConvertible.

                Assert.AreEqual(dataConvert.GetType(), l.GetType());

            }
            catch (Exception ex)
            {

                throw;
            }
            Assert.Pass();
        }
        
        [Test]
        public void Test_RedisDataT()
        {
            try
            {
                //store
                var l = new List<StudentName>();
                l.Add(new StudentName { Id = 1, Name = "one" });
                l.Add(new StudentName { Id = 2, Name = "two" });
                //convert to redis type
                var lr = new RedisData<List<StudentName>>();
                lr.DataType = l.GetType();
                lr.Data = l;
                var rs = JsonConvert.SerializeObject(lr);

                //get
                var redisJsonData = JsonConvert.DeserializeObject<RedisData<List<StudentName>>>(rs);
                var dataConvert = Convert.ChangeType(redisJsonData.Data, redisJsonData.DataType);//https://referencesource.microsoft.com/#mscorlib/system/convert.cs,441ea31c17007e78

                Assert.AreEqual(dataConvert.GetType(), l.GetType());

            }
            catch (Exception ex)
            {

                throw;
            }
            Assert.Pass();
        }

        [Test]
        public void Test_()
        {

            Assert.Pass();
        }
        
    }
    internal class StudentName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
