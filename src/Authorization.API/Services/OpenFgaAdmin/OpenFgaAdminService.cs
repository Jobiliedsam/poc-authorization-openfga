using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Authorization.API.Services.Authorization;
using Authorization.API.Services.Shared;

namespace Authorization.API.Services.OpenFgaAdmin;

public class OpenFgaAdminService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IOpenFgaAdminService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("OpenFga");
    private readonly IConfiguration _configuration = configuration;
    
    public async Task<CreateStoreResponse> InitializeStoreAsync(CreateStoreRequest createStoreRequest)
    {
        string jsonContent = JsonSerializer.Serialize(createStoreRequest, AuthorizationJsonContext.Default.CreateStoreRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/stores", content);
        response.EnsureSuccessStatusCode();
    
        var responseContent = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync(
            responseContent, 
            AuthorizationJsonContext.Default.CreateStoreResponse);
    
        return result;
    }

    public async Task<CreateAuthorizationModelResponse> CreateAuthorizationModelAsync(CreateAuthorizationModelRequest modelRequest, string storeId)
    {
        var openFgaModel = ConvertToOpenFgaModel(modelRequest);
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = false, // Para gerar JSON em uma única linha
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ignorar propriedades nulas
            TypeInfoResolver = AuthorizationJsonContext.Default
        };
        
        string jsonContent = JsonSerializer.Serialize(openFgaModel, typeof(OpenFgaStoreModel), options);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"/stores/{storeId}/authorization-models", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync(
            responseContent, 
            AuthorizationJsonContext.Default.CreateAuthorizationModelResponse);

        return result;
    }

    public async Task WriteRelationshipsAsync(CreateRelationshipRequest request, string storeId)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = false, // Para gerar JSON em uma única linha
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ignorar propriedades nulas
            TypeInfoResolver = AuthorizationJsonContext.Default
        };
        
        var tupleKey = new TupleKeyModel(request.User, request.Relation, $"{request.ObjectType}:{request.ObjectId}");
        var writeRequest = new OpenFgaRelationshipCreationRequest(new OpenFgaWrite(new TupleKeyModel[] { tupleKey }));
        
        string jsonContent = JsonSerializer.Serialize(writeRequest, typeof(OpenFgaRelationshipCreationRequest), options);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"/stores/{storeId}/write", content);
        response.EnsureSuccessStatusCode();
    }
    
    /// <summary>
    /// Converte o modelo de autorização para o formato OpenFGA 1.1
    /// </summary>
    public OpenFgaStoreModel ConvertToOpenFgaModel(CreateAuthorizationModelRequest modelRequest)
    {
        var typeDefinitions = new List<OpenFgaTypeDefinition>();

        foreach (var typeDef in modelRequest.TypeDefinitions)
        {
            var openFgaTypeDef = new OpenFgaTypeDefinition
            {
                Type = typeDef.Type
            };

            // Se o tipo tem relações, processá-las
            if (typeDef.Relations != null && typeDef.Relations.Any())
            {
                
                var metadata = new OpenFgaMetadata
                {
                    Relations = new Dictionary<string, OpenFgaRelationMetadata>()
                };

                foreach (var relation in typeDef.Relations)
                {
                    // Adicionar a relação básica
                    openFgaTypeDef.Relations.Add(relation.Name, new OpenFgaRelation());

                    // Adicionar os metadados da relação
                    if (relation.RewriteRules != null && relation.RewriteRules.Any())
                    {
                        var relatedUserTypes = new List<OpenFgaRelatedUserType>();

                        foreach (var rule in relation.RewriteRules)
                        {
                            // Determinar o tipo relacionado a partir das regras
                            string relatedType = null;
                            if (!string.IsNullOrEmpty(rule.TupleSet))
                            {
                                relatedType = rule.TupleSet;
                            }
                            else if (!string.IsNullOrEmpty(rule.ComputedUserset))
                            {
                                relatedType = rule.ComputedUserset;
                            }

                            if (!string.IsNullOrEmpty(relatedType))
                            {
                                relatedUserTypes.Add(new OpenFgaRelatedUserType
                                {
                                    Type = relatedType
                                });
                            }
                        }

                        metadata.Relations.Add(relation.Name, new OpenFgaRelationMetadata
                        {
                            DirectlyRelatedUserTypes = relatedUserTypes
                        });
                    }
                }

                openFgaTypeDef.Metadata = metadata.Relations.Any() ? metadata : null;
            }

            typeDefinitions.Add(openFgaTypeDef);
        }

        return new OpenFgaStoreModel(typeDefinitions);
    }
}