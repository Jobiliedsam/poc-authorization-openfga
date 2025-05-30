using Authorization.API.Services.Authorization;

namespace Authorization.API.Services.OpenFgaAdmin;

public interface IOpenFgaAdminService
{
    Task<CreateStoreResponse> InitializeStoreAsync(CreateStoreRequest createStoreRequest);
    Task<CreateAuthorizationModelResponse> CreateAuthorizationModelAsync(CreateAuthorizationModelRequest mode, string storeId);
    Task WriteRelationshipsAsync(CreateRelationshipRequest request, string stroeId);
}