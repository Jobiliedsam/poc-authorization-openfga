using System.Text.Json.Serialization;

namespace Authorization.API.Services.OpenFgaAdmin;

public class CreateStoreRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}