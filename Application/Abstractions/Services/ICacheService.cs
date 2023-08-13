namespace Application.Abstractions.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken token);
    Task SetAsync<T>(string key, T data, CancellationToken token);
}
