using Authorization.API.Services.OpenFgaAdmin;

namespace Authorization.API.Services.Authorization;

public class CheckAuthorizationRequest
{
    public string UserId { get; set; } 
    public string Relation { get; set; } = string.Empty;
    public string ObjectType { get; set; } = string.Empty;
    public string ObjectId { get; set; } = string.Empty;

    public static  implicit operator OpenFgaAuthorizationService.CheckAuthorization(CheckAuthorizationRequest request)
    {
        string @object = $"{request.ObjectType}:{request.ObjectId}";
        var tupleKey = new TupleKeyModel(request.UserId, request.Relation, @object);
        
        return new OpenFgaAuthorizationService.CheckAuthorization(tupleKey); 
    }
}