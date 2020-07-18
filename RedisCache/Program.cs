using StackExchange.Redis;
using System;

namespace RedisCache
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();

            string key = string.Format("key{0}", new Random().Next(1, 100000).ToString());
            //SetKeyValue(db, key, string.Format("Value{0}", key));
            //GetKeyValue(db, key);
            UpdateValueByKey(db, "key21447", "test001");//key21447
            GetKeyValue(db, "key21447");

            Console.WriteLine("Hello World!");
        }

        static void GetKeyValue(IDatabase db, string key)
        {
            var val = db.StringGet(key);
            Console.WriteLine(val);
        }

        static void SetKeyValue(IDatabase db, string key, string value)
        {
            var success = db.StringSet(key, value);
            Console.WriteLine(success);
        }

        static void UpdateValueByKey(IDatabase db, string key, string value)
        {
            var success = db.StringSet(key, value);
            Console.WriteLine(success);
        }
    }
}
