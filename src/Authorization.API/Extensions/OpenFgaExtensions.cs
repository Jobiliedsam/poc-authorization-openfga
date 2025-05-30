using System.Net.Http.Headers;
using Authorization.API.Services.Authorization;
using Authorization.API.Services.OpenFgaAdmin;

namespace Authorization.API.Extensions;

public static class OpenFgaExtensions
{
    public static IServiceCollection AddOpenFgaClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("OpenFga", client =>
        {
            client.BaseAddress =
                new Uri(configuration.GetValue<string>("OpenFGA:ApiEndpoint") ?? "http://localhost:8080");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        });
        
        services.AddScoped<IAuthorizationService, OpenFgaAuthorizationService>();
        services.AddScoped<IOpenFgaAdminService, OpenFgaAdminService>();
        
        return services;
    }
}