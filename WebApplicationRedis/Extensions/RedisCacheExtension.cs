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
            string jv = JsonConvert.SerializeObject(value);
            IDatabase db = connectionMultiplexer.GetDatabase(dbNo);
            db.StringSet(key, jv);
        }
        
        public static T GetData<T>(this IConnectionMultiplexer connectionMultiplexer, string key, int dbNo = 0)
        {
            var ret = default(T);
            if (key != null)
            {
                IDatabase db = connectionMultiplexer.GetDatabase(dbNo);
                RedisValue s = db.StringGet(key);

                if (s.HasValue)
                {
                    try
                    {
                        ret = JsonConvert.DeserializeObject<T>(s);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return ret;
        }
        
        public static bool DeleteKey(this IConnectionMultiplexer connectionMultiplexer, int? dbNo = null, params string[] keys)
        {
            bool ret = false;

            if (keys != null)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] == null)
                    {
                        return ret;
                    }
                }

                int rdb = 0;
                if (dbNo != null)
                {
                    rdb = (int)dbNo;
                }

                IDatabase db = connectionMultiplexer.GetDatabase(rdb);

                RedisKey[] rk = keys.Select(x => (RedisKey)x).ToArray();

                ret = db.KeyDelete(rk) > 0;
            }
            return ret;
        }
    }
}
