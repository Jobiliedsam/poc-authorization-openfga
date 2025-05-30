using System.Text.Json.Serialization;

namespace Authorization.API.Services.OpenFgaAdmin;

public sealed record CreateAuthorizationModelResponse()
{
    public string Message { get; set; }
    [JsonPropertyName("authorization_model_id")]
    public string ModelId { get; set; }
}