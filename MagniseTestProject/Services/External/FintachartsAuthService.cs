using System.Text.Json;
using MagniseTask.Data;
using MagniseTask.Interfaces;
using Microsoft.Extensions.Options;

namespace MagniseTask.Services;

public class FintachartsAuthService : IFintachartsAuthService
{
    private readonly HttpClient _httpClient;
    private readonly UserCredentials _userCredentials;
    
    public FintachartsAuthService(IHttpClientFactory httpClientFactory, IOptions<UserCredentials> userCredentials)
    {
        _httpClient = httpClientFactory.CreateClient("FintachartsClient");
        _userCredentials = userCredentials.Value;
    }

    public async Task<AuthData> AuthenticateAsync()
    {
        var requestBody = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "app-cli" },
            { "username", _userCredentials.Username },
            { "password", _userCredentials.Password },
        });

        var response = await _httpClient.PostAsync("/identity/realms/fintatech/protocol/openid-connect/token", requestBody);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
      
        var authResponse = JsonSerializer.Deserialize<AuthData>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        });

        if (authResponse == null)
            return new AuthData();
            
        return authResponse;
    }
}
