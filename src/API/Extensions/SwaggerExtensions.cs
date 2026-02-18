using Microsoft.OpenApi.Models;

namespace JuliGastos.API.Extensions;

/// <summary>
/// Extension methods for configuring Swagger/OpenAPI documentation
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds and configures Swagger with JWT Bearer authentication support
    /// </summary>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Definir el esquema de seguridad JWT Bearer
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingresa tu token JWT en el formato: Bearer {tu token}"
            });

            // Requerir el esquema de seguridad globalmente para todos los endpoints
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        });

        return services;
    }
}
