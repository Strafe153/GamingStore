using Application.Abstractions.Services;
using Domain.Shared;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly CacheOptions _cacheOptions;
    private readonly ILogger<CacheService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public CacheService(
        IDistributedCache cache,
        IOptions<CacheOptions> cacheOptions,
        ILogger<CacheService> logger)
    {
        _cache = cache;
        _cacheOptions = cacheOptions.Value;
        _logger = logger;

        _serializerOptions = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken token)
    {
        var cachedData = await _cache.GetStringAsync(key, token);

        if (cachedData is not null)
        {
            var result = JsonSerializer.Deserialize<T>(cachedData)!;
            _logger.LogInformation("Successfully retrieved cached data of type '{Type}'", typeof(T));

            return result;
        }

        _logger.LogInformation("Cached data of type '{Type}' does not exist", typeof(T));

        return default;
    }

    public async Task SetAsync<T>(string key, T data, CancellationToken token)
    {
        var serializedData = JsonSerializer.Serialize(data, _serializerOptions);

        await _cache.SetStringAsync(key, serializedData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheOptions.AbsoluteExpirationRelativeToNow
        }, token);

        _logger.LogInformation("Successfully cached data of type '{Type}'", typeof(T));
    }
}
