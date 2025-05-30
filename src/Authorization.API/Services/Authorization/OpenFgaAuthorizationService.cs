using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Authorization.API.Services.OpenFgaAdmin;
using Authorization.API.Services.Shared;

namespace Authorization.API.Services.Authorization;

public class OpenFgaAuthorizationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    : IAuthorizationService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("OpenFga");
    private readonly IConfiguration _configuration = configuration;

    

    public async Task<CheckResponse> CheckAuthorizationAsync(CheckAuthorization request, string storeId)
    {
        string jsonContent = JsonSerializer.Serialize(request, AuthorizationJsonContext.Default.CheckAuthorization);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"/stores/{storeId}/check", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync(
            responseContent, 
            AuthorizationJsonContext.Default.CheckResponse);
        
        return result;
    }

    public class CheckAuthorization(TupleKeyModel tupleKey)
    {
        [JsonPropertyName("tuple_key")] public TupleKeyModel TupleKey { get; set; } = tupleKey;
    }
}