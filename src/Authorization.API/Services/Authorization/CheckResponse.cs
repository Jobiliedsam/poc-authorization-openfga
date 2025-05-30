using System.Text.Json.Serialization;

namespace Authorization.API.Services.Authorization;

public class CheckResponse
{
    [JsonPropertyName("allowed")]
    public bool Allowed { get; set; }
}