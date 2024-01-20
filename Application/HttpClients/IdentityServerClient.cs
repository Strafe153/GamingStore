using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Application.HttpClients;

public class IdentityServerClient
{
    private readonly HttpClient _httpClient;

    public IdentityServerClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CheckIdentityServerHealth() =>
        await _httpClient.PostAsync(
            $"{_httpClient.BaseAddress}/connect/token",
            JsonContent.Create(JsonConvert.SerializeObject("{}")));
}