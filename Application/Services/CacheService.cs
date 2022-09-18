using Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _serializerOptions;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
            _serializerOptions = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            byte[] cachedData = await _cache.GetAsync(key);

            if (cachedData is not null)
            {
                string serializedData = Encoding.UTF8.GetString(cachedData);
                var result = JsonSerializer.Deserialize<T>(serializedData)!;

                return result;
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T data)
        {
            string serializedData = JsonSerializer.Serialize(data, _serializerOptions);
            byte[] dataAsBytes = Encoding.UTF8.GetBytes(serializedData);

            await _cache.SetAsync(key, dataAsBytes, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
                SlidingExpiration = TimeSpan.FromMinutes(3)
            });
        }
    }
}
