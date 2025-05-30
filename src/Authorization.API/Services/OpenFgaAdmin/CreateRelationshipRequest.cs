namespace Authorization.API.Services.OpenFgaAdmin;

public class CreateRelationshipRequest
{
    public string User { get; set; } = string.Empty;
    public string Relation { get; set; } = string.Empty;
    public string ObjectType { get; set; } = string.Empty;
    public string ObjectId { get; set; } = string.Empty;
}