using AuthWebApp.DTOs;

namespace AuthWebApp.Services;

public class AuthApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AuthApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        var baseUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        client.BaseAddress = new Uri(baseUrl);

        var response = await client.PostAsJsonAsync("api/auth/register", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AuthResponse>() ?? throw new Exception("Invalid response");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        var baseUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        client.BaseAddress = new Uri(baseUrl);

        var response = await client.PostAsJsonAsync("api/auth/login", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AuthResponse>() ?? throw new Exception("Invalid response");
    }
}