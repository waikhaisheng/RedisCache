using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationRedis.Extensions
{
    public static class RedisCacheExtension
    {
        public static void SetData<T>(this IConnectionMultiplexer connectionMultiplexer, string key, T value, int dbNo = 0)
        {
            var jv = JsonConvert.SerializeObject(value);
            var db = connectionMultiplexer.GetDatabase(dbNo);
            db.StringSet(key, jv);
        }
        
        public static T GetData<T>(this IConnectionMultiplexer connectionMultiplexer, string key, int dbNo = 0)
        {
            var db = connectionMultiplexer.GetDatabase(dbNo);
            var s = db.StringGet(key);

            if (s.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(s);
            }

            return default(T);
        }
    }
}
