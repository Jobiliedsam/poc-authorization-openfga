using System.Text.Json.Serialization;
using Authorization.API.Services.Authorization;
using Authorization.API.Services.OpenFgaAdmin;

namespace Authorization.API.Services.Shared;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(CreateAuthorizationModelRequest))]
[JsonSerializable(typeof(TypeDefinition))]
[JsonSerializable(typeof(RelationDefinition))]
[JsonSerializable(typeof(RewriteRule))]
[JsonSerializable(typeof(CreateRelationshipRequest))]
[JsonSerializable(typeof(CreateStoreResponse))]
[JsonSerializable(typeof(CheckResponse))]
[JsonSerializable(typeof(CheckAuthorizationRequest))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(Token))]
[JsonSerializable(typeof(CreateStoreRequest))]
[JsonSerializable(typeof(Object))]
[JsonSerializable(typeof(CreateAuthorizationModelResponse))]
[JsonSerializable(typeof(ErrorResponse))]
[JsonSerializable(typeof(OpenFgaStoreModel))]
[JsonSerializable(typeof(OpenFgaTypeDefinition))]
[JsonSerializable(typeof(OpenFgaRelation))]
[JsonSerializable(typeof(OpenFgaMetadata))]
[JsonSerializable(typeof(OpenFgaRelationMetadata))]
[JsonSerializable(typeof(OpenFgaRelatedUserType))]
[JsonSerializable(typeof(Dictionary<string, OpenFgaRelation>))]
[JsonSerializable(typeof(Dictionary<string, OpenFgaRelationMetadata>))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(TupleKeyModel))]
[JsonSerializable(typeof(OpenFgaWrite))]
[JsonSerializable(typeof(OpenFgaRelationshipCreationRequest))]
[JsonSerializable(typeof(OpenFgaAuthorizationService.CheckAuthorization))]
[JsonSerializable(typeof(CheckResponse))]
public partial class AuthorizationJsonContext : JsonSerializerContext
{                                               
}                                                                                                                                                                                                                                                   