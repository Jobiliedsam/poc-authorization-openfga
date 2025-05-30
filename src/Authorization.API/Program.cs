using System.Security.Claims;
using Authorization.API.Extensions;
using Authorization.API.Services.Authorization;
using Authorization.API.Services.OpenFgaAdmin;
using Authorization.API.Services.Shared;
using Authorization.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateSlimBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenFgaClient(configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddJwtAuthentication(configuration);

builder.Services.AddSingleton<IOperationFilter, OpenFGAHeaderOperationFilter>();

builder.Services.Configure<RouteOptions>(options => 
    options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = AuthorizationJsonContext.Default;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

#region Admin

var openFgaGroup = app.MapGroup("/api/fga");

openFgaGroup.MapPost("/store", async (CreateStoreRequest createStoreRequest, IOpenFgaAdminService openFgaAdminService) =>
    {
        var result = await openFgaAdminService.InitializeStoreAsync(createStoreRequest);
        result.Message = "OpenFGA store initialized";
        return Results.Ok(result);
    })
    .WithName("InitOpenFGAStore")
    .WithOpenApi(operation => {
        operation.Description = "Initializes a new OpenFGA store and returns its ID";
        return operation;
    });

// Rota para adicionar modelo de autorização
openFgaGroup.MapPost("/authorization-model", async (CreateAuthorizationModelRequest modelRequest, HttpRequest request, 
        IOpenFgaAdminService openFgaAdminService) =>
    {
        var storeId = request.GetStoreIdFromHeader();
        if (string.IsNullOrEmpty(storeId))
        {
            return Results.BadRequest(new ErrorResponse("X-OpenFGA-Store-ID header is required"));
        }
    
        var result = await openFgaAdminService.CreateAuthorizationModelAsync(modelRequest, storeId);
        return Results.Ok(result);
    })
    .WithName("CreateAuthorizationModel")
    .WithOpenApi();

// Rota para adicionar relações
openFgaGroup.MapPost("/relationships", async (CreateRelationshipRequest request, HttpRequest httpRequest, 
        IOpenFgaAdminService openFgaAdminService) =>
    {
        var storeId =  httpRequest.GetStoreIdFromHeader();
        if (string.IsNullOrEmpty(storeId))
        {
            return Results.BadRequest(new ErrorResponse("X-OpenFGA-Store-ID header is required"));
        }
    
        await openFgaAdminService.WriteRelationshipsAsync(request, storeId);
        return Results.Ok();
    })
    .WithName("WriteRelationships")
    .WithOpenApi();
#endregion

#region Authorization

var authGroup = app.MapGroup("/api/authorization");

// Rota para verificar autorização
authGroup.MapGet("/check", async (
    [AsParameters] CheckAuthorizationRequest request,
    HttpContext context,
    HttpRequest httpRequest,
    IAuthorizationService authService) =>
    {
        var storeId = httpRequest.GetStoreIdFromHeader();
        if (string.IsNullOrEmpty(storeId))
        {
            return Results.BadRequest(new ErrorResponse("X-OpenFGA-Store-ID header is required"));
        }

        var allowed = await authService.CheckAuthorizationAsync(request, storeId);
        return Results.Ok(allowed);
    })
    .WithName("CheckAuthorization")
    .WithOpenApi();

// Rota de login para obter token JWT para testes
app.MapPost("/api/login", (LoginRequest login) =>
    {
        if (login.Username == "admin" && login.Password == "admin123")
        {
            var token = app.GenerateToken(login.Username, new[] { "Admin" });
            return Results.Ok(new Token(token));
        }
        else if (login.Username == "user" && login.Password == "user123")
        {
            var token = app.GenerateToken(login.Username, new[] { "User" });
            return Results.Ok(new Token(token));
        }

        return Results.Unauthorized();
    })
    .AllowAnonymous()
    .WithName("Login")
    .WithOpenApi();


#endregion

app.Run();



