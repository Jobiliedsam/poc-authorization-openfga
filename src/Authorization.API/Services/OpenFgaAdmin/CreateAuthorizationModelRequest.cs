namespace Authorization.API.Services.OpenFgaAdmin;

public class CreateAuthorizationModelRequest
{
    public List<TypeDefinition> TypeDefinitions { get; set; } = new List<TypeDefinition>();
}

public class TypeDefinition
{
    public string Type { get; set; } = string.Empty;
    public List<RelationDefinition> Relations { get; set; } = new();
}

public class RelationDefinition
{
    public string Name { get; set; } = string.Empty;
    public List<RewriteRule>? RewriteRules { get; set; }
}

public class RewriteRule
{
    public string? ThisType { get; set; }
    public string? ComputedUserset { get; set; }
    public string? TupleSet { get; set; }
}