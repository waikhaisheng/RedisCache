using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApplicationRedis.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            DistributedCacheEntryOptions option = new DistributedCacheEntryOptions();
            option.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(600);
            option.SlidingExpiration = unusedExpireTime;

            string jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, option);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            string jsonData = await cache.GetStringAsync(recordId);

            if (jsonData == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
