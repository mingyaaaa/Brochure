using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Brochure.Core.Server.Extensions
{
    public static class IDistributedCacheExtensions
    {
        public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var str = await distributedCache.GetStringAsync(key);
            return JsonSerializer.Deserialize<T>(str, jsonSerializerOptions)!;
        }

        public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value)
        {
            var str = JsonSerializer.Serialize<T>(value)!;
            await distributedCache.SetStringAsync(key, str);
        }

        public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions distributedCacheEntryOptions)
        {
            var str = JsonSerializer.Serialize<T>(value)!;
            await distributedCache.SetStringAsync(key, str, distributedCacheEntryOptions);
        }
    }
}