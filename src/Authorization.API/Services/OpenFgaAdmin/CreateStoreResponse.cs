using System.Text.Json.Serialization;

namespace Authorization.API.Services.OpenFgaAdmin;

public sealed class CreateStoreResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; } = string.Empty;
}