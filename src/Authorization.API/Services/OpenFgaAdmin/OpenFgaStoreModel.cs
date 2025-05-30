using System.Text.Json.Serialization;

namespace Authorization.API.Services.OpenFgaAdmin;

public class OpenFgaStoreModel
{
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; set; } = "1.1";

    [JsonPropertyName("type_definitions")]
    public List<OpenFgaTypeDefinition> TypeDefinitions { get; set; }

    public OpenFgaStoreModel(List<OpenFgaTypeDefinition> typeDefinitions)
    {
        TypeDefinitions = typeDefinitions;
    }
}

public class OpenFgaTypeDefinition
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("relations")] public Dictionary<string, OpenFgaRelation> Relations { get; set; } = new();

    [JsonPropertyName("metadata")]
    public OpenFgaMetadata Metadata { get; set; }
}

public class OpenFgaRelation
{
    [JsonPropertyName("this")]
    public object This { get; set; } = new object();

}

public class OpenFgaMetadata
{
    [JsonPropertyName("relations")]
    public Dictionary<string, OpenFgaRelationMetadata> Relations { get; set; }
}

public class OpenFgaRelationMetadata
{
    [JsonPropertyName("directly_related_user_types")]
    public List<OpenFgaRelatedUserType> DirectlyRelatedUserTypes { get; set; }
}

public class OpenFgaRelatedUserType
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}