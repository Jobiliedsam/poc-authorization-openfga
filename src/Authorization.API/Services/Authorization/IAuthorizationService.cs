namespace Authorization.API.Services.Authorization;

public interface IAuthorizationService
{
    Task<CheckResponse> CheckAuthorizationAsync(OpenFgaAuthorizationService.CheckAuthorization request, string storeId);
}