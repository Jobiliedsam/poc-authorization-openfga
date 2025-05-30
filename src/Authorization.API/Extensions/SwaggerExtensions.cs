using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Authorization.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { 
                Title = "Authorization API", 
                Version = "v1",
                Description = "API for managing permissions using OpenFGA" 
            });
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            
            c.OperationFilter<OpenFGAHeaderOperationFilter>();
        });
        
        return services;
    }
    
    public static WebApplication UseSwaggerExtension(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        return app;
    }
}

public class OpenFGAHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Pula os endpoints que não precisam do header
        if (context.ApiDescription.RelativePath?.Contains("store") == true ||
            context.ApiDescription.RelativePath?.Contains("login") == true)
        {
            return;
        }
        
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();
            
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-OpenFGA-Store-ID",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string"
            },
            Description = "The ID of the OpenFGA store to use for this operation"
        });
    }
}